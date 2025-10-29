using DAT.Entity;
using DAT.UnitOfWork;
using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // Cần dùng

namespace BUS.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<OrderDTO>> GetAllAsync()
        {
            // SỬA: Dùng Repository chuyên dụng
            var orders = await _unitOfWork.Orders.GetAllWithDetailsAsync();

            return orders.Select(o => new OrderDTO
            {
                OrderID = o.OrderID,
                UserID = o.UserID,
                AdrsID = o.AdrsID,
                RestaurantID = o.RestaurantID,
                OrderTime = o.OrderTime,
                StatusID = o.StatusID,
                StatusName = o.OrderStatus?.StatusName, // Sẽ có dữ liệu
                TotalAmount = o.TotalAmount,
                UpdatedAt = o.UpdatedAt,
                Items = o.OrderItems.Select(i => new OrderItemDTO
                {
                    FoodID = i.FoodID,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            });
        }

        public async Task<OrderDTO?> GetByIdAsync(int id) // SỬA: long -> int
        {
            // SỬA: Dùng Repository chuyên dụng
            var o = await _unitOfWork.Orders.GetByIdWithDetailsAsync(id);
            if (o == null) return null;

            return new OrderDTO
            {
                OrderID = o.OrderID,
                UserID = o.UserID,
                AdrsID = o.AdrsID,
                RestaurantID = o.RestaurantID,
                OrderTime = o.OrderTime,
                StatusID = o.StatusID,
                StatusName = o.OrderStatus?.StatusName, // Sẽ có dữ liệu
                TotalAmount = o.TotalAmount,
                UpdatedAt = o.UpdatedAt,
                Items = o.OrderItems.Select(i => new OrderItemDTO // Sẽ có dữ liệu
                {
                    FoodID = i.FoodID,
                    Quantity = i.Quantity,
                    Price = i.Price // SỬA: UnitPrice -> Price
                }).ToList(),
            };
        }

        public async Task<int> CreateAsync(OrderDTO dto) // SỬA: long -> int
        {
            // --- SỬA LOGIC: Lấy StatusID từ tên "Pending" ---
            var pendingStatus = (await _unitOfWork.Repository<OrderStatus>()
                                .FindAsync(s => s.StatusName == "Pending"))
                                .FirstOrDefault();

            if (pendingStatus == null)
            {
                // Nếu chưa có Status "Pending", tạo nó (hoặc báo lỗi)
                // Tạm thời, giả sử ID của "Pending" là 1
                // throw new Exception("OrderStatus 'Pending' not found.");
                pendingStatus = new OrderStatus { StatusID = 1, StatusName = "Pending" }; // Cẩn thận!
            }

            // Tính tổng tiền
            int total = 0;
            foreach (var item in dto.Items)
            {
                // SỬA: Dùng Repository chuyên dụng
                var food = await _unitOfWork.FoodItems.GetByIdAsync(item.FoodID);
                if (food == null) throw new Exception($"Food item not found: {item.FoodID}");

                item.Price = food.Price; // Gán giá đúng vào DTO
                total += food.Price * item.Quantity;
            }

            var order = new Order
            {
                UserID = dto.UserID,
                AdrsID = dto.AdrsID,
                RestaurantID = dto.RestaurantID, // THÊM
                OrderTime = DateTime.UtcNow,
                StatusID = pendingStatus.StatusID, // SỬA: Dùng ID
                TotalAmount = total,
                // PromoId = dto.PromoId, // XÓA
                OrderItems = dto.Items.Select(i => new OrderItem
                {
                    FoodID = i.FoodID,
                    Quantity = i.Quantity,
                    Price = i.Price // SỬA: UnitPrice -> Price
                }).ToList(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            // SỬA: Dùng Repository chuyên dụng
            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return order.OrderID;
        }

        public async Task UpdateStatusAsync(int orderId, string status) // SỬA: long -> int
        {
            // SỬA LOGIC: Phải tìm ID từ tên status
            var statusEntity = (await _unitOfWork.Repository<OrderStatus>()
                                .FindAsync(s => s.StatusName == status))
                                .FirstOrDefault();

            if (statusEntity == null)
                throw new Exception($"Status '{status}' not found.");

            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (order == null)
                throw new Exception("Order not found");

            order.StatusID = statusEntity.StatusID; // SỬA
            order.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Orders.Update(order);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id) // SỬA: long -> int
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null) return false;

            // Cẩn thận: Xóa Order thường phải xóa OrderItem trước
            // Nhưng nếu CSDL cài "ON DELETE CASCADE" thì EF Core sẽ tự xử lý
            _unitOfWork.Orders.Remove(order);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}