using DAT.UnitOfWork;
using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAT.Entity; // <-- Đảm bảo bạn đã using Entity
using Microsoft.EntityFrameworkCore; // <-- Cần cho .Include()

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
            var items = await _unitOfWork.FoodItems.GetAllWithDetailsAsync();

            return items.Select(f => new FoodItemDTO
            {
                FoodId = f.FoodID,           // SỬA: FoodId -> FoodID
                FoodName = f.FoodName,
                Description = f.Description,
                Price = f.Price,
                ImgUrl = f.ImageURL,       // SỬA: ImgUrl -> ImageURL
                StatusID = f.StatusID,     // SỬA: Status -> StatusID
                StatusName = f.FoodStatus?.StatusName, // Lấy tên từ bảng liên kết
                CreatedAt = f.CreatedAt,
                UpdatedAt = f.UpdatedAt,
                CategoryId = f.CategoryID,   // SỬA: CategoryId -> CategoryID
                CategoryName = f.Category?.Name
            });
        }

        public async Task<IEnumerable<FoodItemDTO>> GetFoodsByCategoryAsync(int categoryId) // SỬA: long -> int
        {
            var items = await _unitOfWork.FoodItems.GetByCategoryWithDetailsAsync(categoryId);

            // Mã mapping lặp lại, chúng ta có thể tạo 1 hàm private MapToDTO(FoodItem f)
            return items.Select(f => new FoodItemDTO
            {
                FoodId = f.FoodID,           // SỬA: FoodId -> FoodID
                FoodName = f.FoodName,
                Description = f.Description,
                Price = f.Price,
                ImgUrl = f.ImageURL,       // SỬA: ImgUrl -> ImageURL
                StatusID = f.StatusID,     // SỬA: Status -> StatusID
                StatusName = f.FoodStatus?.StatusName,
                CreatedAt = f.CreatedAt,
                UpdatedAt = f.UpdatedAt,
                CategoryId = f.CategoryID,   // SỬA: CategoryId -> CategoryID
                CategoryName = f.Category?.Name
            });
        }

        public async Task<FoodItemDTO?> GetByIdAsync(int id) // SỬA: long -> int
        {
            var f = await _unitOfWork.FoodItems.GetByIdWithDetailsAsync(id);

            if (f == null) return null;

            return new FoodItemDTO
            {
                FoodId = f.FoodID,           // SỬA: FoodId -> FoodID
                FoodName = f.FoodName,
                Description = f.Description,
                Price = f.Price,
                ImgUrl = f.ImageURL,       // SỬA: ImgUrl -> ImageURL
                StatusID = f.StatusID,     // SỬA: Status -> StatusID
                StatusName = f.FoodStatus?.StatusName,
                CreatedAt = f.CreatedAt,
                UpdatedAt = f.UpdatedAt,
                CategoryId = f.CategoryID,   // SỬA: CategoryId -> CategoryID
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
                ImageURL = dto.ImgUrl,       // SỬA: ImgUrl -> ImageURL
                StatusID = dto.StatusID,     // SỬA: Status -> StatusID (Giả sử DTO có StatusID)
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CategoryID = dto.CategoryId  // SỬA: CategoryId -> CategoryID
            };
            await _unitOfWork.FoodItems.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(FoodItemDTO dto)
        {
            var entity = await _unitOfWork.FoodItems.GetByIdAsync(dto.FoodId); // SỬA: long -> int
            if (entity == null) throw new Exception("Food item not found");

            entity.FoodName = dto.FoodName;
            entity.Description = dto.Description;
            entity.Price = dto.Price;
            entity.ImageURL = dto.ImgUrl;       // SỬA: ImgUrl -> ImageURL
            entity.StatusID = dto.StatusID;     // SỬA: Status -> StatusID
            entity.UpdatedAt = DateTime.UtcNow;
            entity.CategoryID = dto.CategoryId; // SỬA: CategoryId -> CategoryID

            _unitOfWork.FoodItems.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id) // SỬA: long -> int
        {
            var entity = await _unitOfWork.FoodItems.GetByIdAsync(id);
            if (entity == null) return false;

            _unitOfWork.FoodItems.Remove(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}