using DAT.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Repository
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<Order>> GetAllWithDetailsAsync(CancellationToken ct = default);
        Task<IEnumerable<Order>> GetByUserIdWithDetailsAsync(int userId, CancellationToken ct = default);
    }
}
