using DAT.UnitOfWork;
using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Services
{
    public class FoodItemService : IFoodItemService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FoodItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<FoodItemDTO>> GetAllAsync()
        {
            var items = await _unitOfWork.FoodItems.GetAllAsync();
            return items.Select(f => new FoodItemDTO
            {
                FoodId = f.FoodId,
                FoodName = f.FoodName,
                Description = f.Description,
                Price = f.Price,
                ImgUrl = f.ImgUrl,
                Status = f.Status,
                CreatedAt = f.CreatedAt,
                UpdatedAt = f.UpdatedAt,
                CategoryId = f.CategoryId,
                CategoryName = f.Category?.Name
            });
        }

        public async Task<IEnumerable<FoodItemDTO>> GetFoodsByCategoryAsync(long categoryId)
        {
            var items = await _unitOfWork.FoodItems.GetAllAsync();
            var filtered = items.Where(f => f.CategoryId == categoryId);

            return filtered.Select(f => new FoodItemDTO
            {
                FoodId = f.FoodId,
                FoodName = f.FoodName,
                Description = f.Description,
                Price = f.Price,
                ImgUrl = f.ImgUrl,
                Status = f.Status,
                CreatedAt = f.CreatedAt,
                UpdatedAt = f.UpdatedAt,
                CategoryId = f.CategoryId,
                CategoryName = f.Category?.Name
            });
        }

        public async Task<FoodItemDTO?> GetByIdAsync(long id)
        {
            var f = await _unitOfWork.FoodItems.GetByIdAsync(id);
            if (f == null) return null;
            return new FoodItemDTO
            {
                FoodId = f.FoodId,
                FoodName = f.FoodName,
                Description = f.Description,
                Price = f.Price,
                ImgUrl = f.ImgUrl,
                Status = f.Status,
                CreatedAt = f.CreatedAt,
                UpdatedAt = f.UpdatedAt,
                CategoryId = f.CategoryId,
                CategoryName = f.Category?.Name
            };
        }

        public async Task AddAsync(FoodItemDTO dto)
        {
            var entity = new DAT.Entity.FoodItem
            {
                FoodName = dto.FoodName,
                Description = dto.Description,
                Price = dto.Price,
                ImgUrl = dto.ImgUrl,
                Status = dto.Status,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CategoryId = dto.CategoryId
            };
            await _unitOfWork.FoodItems.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(FoodItemDTO dto)
        {
            var entity = await _unitOfWork.FoodItems.GetByIdAsync(dto.FoodId);
            if (entity == null) throw new Exception("Food item not found");

            entity.FoodName = dto.FoodName;
            entity.Description = dto.Description;
            entity.Price = dto.Price;
            entity.ImgUrl = dto.ImgUrl;
            entity.Status = dto.Status;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.CategoryId = dto.CategoryId;

            _unitOfWork.FoodItems.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _unitOfWork.FoodItems.GetByIdAsync(id);
            if (entity == null) return false;

            _unitOfWork.FoodItems.Remove(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }

}
