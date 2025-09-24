using DAT.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Repository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<List<Category>> GetAllWithProductsAsync();
    }

    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(FastFoodDbContext context) : base(context) { }

        public async Task<List<Category>> GetAllWithProductsAsync()
        {
            return await _dbSet.Include(c => c.Products).ToListAsync();
        }
    }
}
