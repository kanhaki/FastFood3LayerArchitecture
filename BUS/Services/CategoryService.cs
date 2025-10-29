using DAT.UnitOfWork;
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
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return categories.Select(c => new CategoryDTO
            {
                CategoryId = c.CategoryID,
                Name = c.Name,
                Description = c.Description,
                ImgUrl = c.ImageURL
            });
        }

        public async Task<CategoryDTO?> GetByIdAsync(long id)
        {
            var c = await _unitOfWork.Categories.GetByIdAsync(id);
            if (c == null) return null;
            return new CategoryDTO
            {
                CategoryId = c.CategoryID,
                Name = c.Name,
                Description = c.Description,
                ImgUrl = c.ImageURL
            };
        }

        public async Task AddAsync(CategoryDTO dto)
        {
            var entity = new DAT.Entity.Category
            {
                Name = dto.Name,
                Description = dto.Description,
                ImageURL = dto.ImgUrl
            };
            await _unitOfWork.Categories.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(CategoryDTO dto)
        {
            var entity = await _unitOfWork.Categories.GetByIdAsync(dto.CategoryId);
            if (entity == null) throw new Exception("Category not found");

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.ImageURL = dto.ImgUrl;

            _unitOfWork.Categories.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _unitOfWork.Categories.GetByIdAsync(id);
            if (entity == null) return false;

            _unitOfWork.Categories.Remove(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }

}
