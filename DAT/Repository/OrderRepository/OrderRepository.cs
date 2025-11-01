using DAT.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context) { }

        // Hàm helper để tránh lặp code .Include()
        private IQueryable<Order> GetFullOrderDetails()
        {
            return _dbSet
                .Include(o => o.OrderStatus)
                .Include(o => o.Address)
                .Include(o => o.User)
                .Include(o => o.Restaurant)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.FoodItem); // Lấy cả FoodItem của OrderItem
        }

        public async Task<Order?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default)
        {
            return await GetFullOrderDetails()
                        .SingleOrDefaultAsync(o => o.OrderID == id, ct);
        }

        public async Task<IEnumerable<Order>> GetAllWithDetailsAsync(CancellationToken ct = default)
        {
            return await GetFullOrderDetails()
                        .ToListAsync(ct);
        }

        public async Task<IEnumerable<Order>> GetByUserIdWithDetailsAsync(int userId, CancellationToken ct = default)
        {
            return await GetFullOrderDetails()
                        .Where(o => o.UserID == userId)
                        .ToListAsync(ct);
        }
    }
}
