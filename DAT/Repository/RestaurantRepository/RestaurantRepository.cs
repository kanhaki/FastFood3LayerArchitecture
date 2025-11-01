using DAT.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Repository
{
    public class RestaurantRepository : GenericRepository<Restaurant>, IRestaurantRepository
    {
        public RestaurantRepository(AppDbContext context) : base(context) { }

        // Hàm "thực thi" .Include()
        public async Task<IEnumerable<Restaurant>> GetAllWithStatusAsync()
        {
            return await _dbSet
                        .Include(r => r.RestaurantStatus)
                        .ToListAsync();
        }

        public async Task<Restaurant?> GetByIdWithStatusAsync(int id)
        {
            return await _dbSet
                        .Include(r => r.RestaurantStatus)
                        .SingleOrDefaultAsync(r => r.RestaurantID == id);
        }
    }
}
