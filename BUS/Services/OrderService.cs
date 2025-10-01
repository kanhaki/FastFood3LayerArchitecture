using DAT.Entity;
using DAT.UnitOfWork;
using DTO.DTO;
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

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<OrderDTO>> GetAllAsync()
        {
            var orders = await _unitOfWork.Repository<Order>().GetAllAsync();
            return orders.Select(o => new OrderDTO
            {
                OrderId = o.OrderId,
                UserId = o.UserId,
                AdrsId = o.AddrId,
                OrderTime = o.OrderTime,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
                PromoId = o.PromoId,
                UpdatedAt = o.UpdatedAt,
            });
        }

        public async Task<OrderDTO?> GetByIdAsync(long id)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(id);
            if (order == null) return null;

            return new OrderDTO
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                AdrsId = order.AddrId,
                OrderTime = order.OrderTime,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                PromoId = order.PromoId,
                Items = order.OrderItems.Select(i => new OrderItemDTO
                {
                    OrderItemId = i.OrderItemId,
                    FoodId = i.FoodId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList(),
                UpdatedAt = order.UpdatedAt,
            };
        }

        public async Task<long> CreateAsync(OrderDTO dto)
        {
            // Tính tổng tiền
            int total = 0;
            foreach (var item in dto.Items)
            {
                var food = await _unitOfWork.Repository<FoodItem>().GetByIdAsync(item.FoodId);
                if (food == null) throw new Exception("Food item not found");

                item.UnitPrice = food.Price;
                total += food.Price * item.Quantity;
            }

            var order = new Order
            {
                UserId = dto.UserId,
                AddrId = dto.AdrsId,
                OrderTime = DateTime.UtcNow,
                Status = "Pending",
                TotalAmount = total,
                PromoId = dto.PromoId,
                OrderItems = dto.Items.Select(i => new OrderItem
                {
                    FoodId = i.FoodId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList(),
                UpdatedAt = DateTime.UtcNow,
            };

            await _unitOfWork.Repository<Order>().AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return order.OrderId;
        }

        public async Task UpdateStatusAsync(long orderId, string status)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(orderId);
            if (order == null) throw new Exception("Order not found");

            order.Status = status;
            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(id);
            if (order == null) return false;

            _unitOfWork.Repository<Order>().Remove(order);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }

}
