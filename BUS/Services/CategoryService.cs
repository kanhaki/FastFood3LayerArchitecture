using DAT.Entity;
using DAT.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
            => await _categoryRepository.GetAllAsync();

        public async Task<List<Category>> GetAllWithProductsAsync()
            => await _categoryRepository.GetAllWithProductsAsync();
    }
}
