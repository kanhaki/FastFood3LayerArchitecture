using DAT.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Repository
{
    public class FoodItemRepository : GenericRepository<FoodItem>, IFoodItemRepository
    {
        public FoodItemRepository(AppDbContext context) : base(context) { }

        // Đây là hàm "thực thi" có .Include()
        public async Task<IEnumerable<FoodItem>> GetAllWithDetailsAsync(CancellationToken ct = default)
        {
            return await _dbSet
                        .Include(f => f.Category)
                        .Include(f => f.FoodStatus)
                        .ToListAsync(ct);
        }

        public async Task<FoodItem?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default)
        {
            return await _dbSet
                        .Include(f => f.Category)
                        .Include(f => f.FoodStatus)
                        .SingleOrDefaultAsync(f => f.FoodID == id, ct);
        }

        public async Task<IEnumerable<FoodItem>> GetByCategoryWithDetailsAsync(int categoryId, CancellationToken ct = default)
        {
            return await _dbSet
                       .Where(f => f.CategoryID == categoryId)
                       .Include(f => f.Category)
                       .Include(f => f.FoodStatus)
                       .ToListAsync(ct);
        }
    }
}
