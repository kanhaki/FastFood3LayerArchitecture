using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetAllAsync();
        Task<OrderDTO?> GetByIdAsync(long id);
        Task<long> CreateAsync(OrderDTO dto);
        Task UpdateStatusAsync(long orderId, string status);
        Task<bool> DeleteAsync(long id);
    }
}
