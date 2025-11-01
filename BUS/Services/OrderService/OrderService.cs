using Common;
using DAT.Entity;
using DAT.UnitOfWork;
using DTO.DTO;
using DTO.DTO.Order;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderService(IUnitOfWork unitOfWork, IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IEnumerable<OrderDTO>> GetAllAsync()
        {
            var orders = await _unitOfWork.Orders.GetAllWithDetailsAsync();
            return orders.Select(MapToDTO);
        }
        public async Task<OrderDTO?> GetByIdAsync(int id)
        {
            var o = await _unitOfWork.Orders.GetByIdWithDetailsAsync(id);
            if (o == null) return null;
            return MapToDTO(o);
        }
        public async Task<OrderCreationResponseDTO> CreateAsync(OrderDTO dto, int userIdFromToken)
        {
            var pendingStatus = (await _unitOfWork.Repository<OrderStatus>()
                                .FindAsync(s => s.StatusName == "Pending"))
                                .FirstOrDefault();

            if (pendingStatus == null)
            {
                throw new InvalidOperationException("LỖI HỆ THỐNG: Không tìm thấy 'Pending' Status. Hãy seed CSDL.");
            }

            int total = 0;
            foreach (var item in dto.Items)
            {
                var food = await _unitOfWork.FoodItems.GetByIdAsync(item.FoodID);

                if (food == null)
                    throw new KeyNotFoundException($"Food item not found: {item.FoodID}");

                item.Price = food.Price;
                total += food.Price * item.Quantity;
            }

            var order = new Order
            {
                UserID = userIdFromToken,
                AdrsID = dto.AdrsID,
                RestaurantID = dto.RestaurantID,
                OrderTime = DateTime.UtcNow,
                StatusID = pendingStatus.StatusID,
                TotalAmount = total,
                OrderItems = dto.Items.Select(i => new OrderItem
                {
                    FoodID = i.FoodID,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            string paymentUrl = CreateVnpayPaymentUrl(order, _httpContextAccessor.HttpContext);

            return new OrderCreationResponseDTO
            {
                OrderID = order.OrderID,
                PaymentUrl = paymentUrl
            };
        }

        public async Task<UpdateStatusResult> UpdateStatusAsync(int orderId, string status)
        {
            // Bước 1: Kiểm tra Status
            var statusEntity = (await _unitOfWork.Repository<OrderStatus>()
                                .FindAsync(s => s.StatusName == status))
                                .FirstOrDefault();

            // SỬA: Không "throw", chỉ "báo cáo"
            if (statusEntity == null)
                return UpdateStatusResult.StatusNotFound;

            // Bước 2: Kiểm tra Order
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);

            // SỬA: Không "throw", chỉ "báo cáo"
            if (order == null)
                return UpdateStatusResult.OrderNotFound;

            // Bước 3: Cập nhật và lưu
            order.StatusID = statusEntity.StatusID;
            order.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Orders.Update(order);
            await _unitOfWork.SaveChangesAsync();

            // Báo cáo thành công
            return UpdateStatusResult.Success;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null) return false;

            _unitOfWork.Orders.Remove(order);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        private OrderDTO MapToDTO(Order o)
        {
            return new OrderDTO
            {
                OrderID = o.OrderID,
                UserID = o.UserID,
                AdrsID = o.AdrsID,
                RestaurantID = o.RestaurantID,
                OrderTime = o.OrderTime,
                StatusID = o.StatusID,
                StatusName = o.OrderStatus?.StatusName,
                TotalAmount = o.TotalAmount,
                UpdatedAt = o.UpdatedAt,
                Items = o.OrderItems.Select(i => new OrderItemDTO
                {
                    FoodID = i.FoodID,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList(),
            };
        }
        private string CreateVnpayPaymentUrl(Order order, HttpContext httpContext)
        {
            var vnpayConfig = _config.GetSection("Vnpay");
            var tmnCode = vnpayConfig["TmnCode"];
            var hashSecret = vnpayConfig["HashSecret"];
            var baseUrl = vnpayConfig["BaseUrl"];
            var returnUrl = vnpayConfig["ReturnUrl"];
            var ipnUrl = vnpayConfig["IpnUrl"];

            var pay = new SortedList<string, string>(StringComparer.Ordinal)
            {
                { "vnp_Version", "2.1.0" },
                { "vnp_Command", "pay" },
                { "vnp_TmnCode", tmnCode },
                { "vnp_Amount", (order.TotalAmount * 100).ToString() }, // VNPAY * 100
                { "vnp_CreateDate", order.CreatedAt.ToString("yyyyMMddHHmmss") },
                { "vnp_CurrCode", "VND" },
                { "vnp_IpAddr", GetIpAddress(httpContext) },
                { "vnp_Locale", "vn" },
                { "vnp_OrderInfo", $"Thanh toan don hang {order.OrderID}" },
                { "vnp_OrderType", "other" }, // (Loại hàng hóa)
                { "vnp_ReturnUrl", returnUrl }, // (URL cho User)
                { "vnp_IpnUrl", ipnUrl }, // (URL cho Server-to-Server)
                { "vnp_TxnRef", order.OrderID.ToString() }, // (Mã đơn hàng của BẠN)
                { "vnp_ExpireDate", order.CreatedAt.AddMinutes(15).ToString("yyyyMMddHHmmss") } // Hạn 15 phút
            };

            // Ghép chuỗi (key=value&key=value...)
            var rawDataBuilder = new StringBuilder();
            foreach (var (key, value) in pay)
            {
                rawDataBuilder.Append(key + "=" + value + "&");
            }
            string rawData = rawDataBuilder.ToString().Remove(rawDataBuilder.Length - 1, 1); // Xóa dấu & cuối

            // Hash
            string secureHash = VnpayHelper.HmacSHA512(hashSecret, rawData);

            // Build URL
            return $"{baseUrl}?{rawData}&vnp_SecureHash={secureHash}";
        }

        private string GetIpAddress(HttpContext httpContext)
        {
            string ipAddress = httpContext.Connection.RemoteIpAddress?.ToString();
            if (string.IsNullOrEmpty(ipAddress) || ipAddress == "::1")
            {
                ipAddress = "127.0.0.1"; // (IP mặc định nếu là localhost)
            }
            // (Xử lý nếu IP là IPv6-mapped-IPv4)
            if (ipAddress.StartsWith("::ffff:"))
            {
                ipAddress = ipAddress.Substring(7);
            }
            return ipAddress;
        }
    }
}