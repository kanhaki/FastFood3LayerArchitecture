using DAT.Entity;
using DAT.UnitOfWork;
using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
            var orders = await _unitOfWork.Orders.GetAllWithDetailsAsync();
            return orders.Select(MapToDTO);
        }
        public async Task<OrderDTO?> GetByIdAsync(int id)
        {
            var o = await _unitOfWork.Orders.GetByIdWithDetailsAsync(id);
            if (o == null) return null;
            return MapToDTO(o);
        }
        public async Task<int> CreateAsync(OrderDTO dto, int userIdFromToken)
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

            return order.OrderID;
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
    }
}