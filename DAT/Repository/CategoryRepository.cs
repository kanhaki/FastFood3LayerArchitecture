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
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int categoryId);
    }

    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {

        public CategoryRepository(FastFoodDbContext context) : base(context) { }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            // Lấy tất cả danh mục, không bắt buộc load sản phẩm nếu muốn
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            // Lấy danh mục kèm sản phẩm
            return await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
        }

    }
}
