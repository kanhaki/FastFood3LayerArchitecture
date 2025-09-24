using AutoMapper;
using DAT.Entity;
using DAT.Repository;
using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<CategoryDTO>> GetAllCategoriesAsync()
        {
            var categories = await _repo.GetAllCategoriesAsync();
            return _mapper.Map<List<CategoryDTO>>(categories);
        }

        public async Task<CategoryDTO> GetCategoryByIdAsync(int categoryId)
        {
            var category = await _repo.GetCategoryByIdAsync(categoryId);
            return _mapper.Map<CategoryDTO>(category);
        }
    }
}
