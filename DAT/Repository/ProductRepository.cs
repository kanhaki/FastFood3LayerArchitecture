using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAT.Entity;
using DAT.Repository;
using Microsoft.EntityFrameworkCore;

namespace DAT.Repository
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<List<Product>> GetComboProductsAsync();
        Task<List<Product>> GetProductsByCategoryAsync(int categoryId, bool isCombo);
        Task<Product> GetProductByIdAsync(int productId);
    }
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(FastFoodDbContext context) : base(context) { }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _dbSet.Include(p => p.Category).ToListAsync();
        }

        public async Task<List<Product>> GetComboProductsAsync()
        {
            return await _dbSet
                .Include(p => p.ComboDetails)
                .Include(p => p.ComboOptionGroups)
                    .ThenInclude(g => g.ComboOptionItems)
                .Where(p => p.IsCombo)
                .ToListAsync();
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId, bool isCombo)
        {
            return await _dbSet
                .Where(p => p.CategoryId == categoryId && p.IsCombo == isCombo)
                .ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _context.Products
                .Include(p => p.ComboOptionGroups)
                    .ThenInclude(g => g.ComboOptionItems)
                        .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(p => p.ProductId == productId);
        }
    }
}
