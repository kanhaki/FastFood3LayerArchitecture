using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Services
{
    public enum UpdateStatusResult
    {
        Success,
        OrderNotFound,
        StatusNotFound
    }
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetAllAsync();
        Task<OrderDTO?> GetByIdAsync(int id);
        Task<int> CreateAsync(OrderDTO dto, int userIdFromToken);

        Task<UpdateStatusResult> UpdateStatusAsync(int orderId, string status);
        Task<bool> DeleteAsync(int id);
    }
}
