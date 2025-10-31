using BUS.Services;
using DAT.Entity; // <-- Đảm bảo using
using DAT.UnitOfWork;
using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        // CẢI TIẾN: Dùng hàm MapToDTO
        return items.Select(MapToDTO);
    }

    public async Task<IEnumerable<FoodItemDTO>> GetFoodsByCategoryAsync(int categoryId)
    {
        var items = await _unitOfWork.FoodItems.GetByCategoryWithDetailsAsync(categoryId);
        // CẢI TIẾN: Dùng hàm MapToDTO
        return items.Select(MapToDTO);
    }

    public async Task<FoodItemDTO?> GetByIdAsync(int id)
    {
        var f = await _unitOfWork.FoodItems.GetByIdWithDetailsAsync(id);
        if (f == null) return null;

        // CẢI TIẾN: Dùng hàm MapToDTO
        return MapToDTO(f);
    }

    public async Task AddAsync(FoodItemDTO dto)
    {
        // Chúng ta cũng có thể tạo hàm MapToEntity, nhưng AddAsync khá đơn giản
        var entity = new DAT.Entity.FoodItem
        {
            FoodName = dto.FoodName,
            Description = dto.Description,
            Price = dto.Price,
            ImageURL = dto.ImgUrl,
            StatusID = dto.StatusID,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CategoryID = dto.CategoryId
        };
        await _unitOfWork.FoodItems.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    // --- CẢI TIẾN LỚN ---
    public async Task<bool> UpdateAsync(FoodItemDTO dto)
    {
        // 1. Lấy entity.
        // Lưu ý: Dùng GetByIdAsync (của Generic) là đủ,
        // vì chúng ta không cần .Include() chi tiết khi chỉ Update.
        var entity = await _unitOfWork.FoodItems.GetByIdAsync(dto.FoodId);

        // 2. Trả về false nếu không tìm thấy. Không throw nữa!
        if (entity == null)
            return false;

        // 3. Ánh xạ các thay đổi
        entity.FoodName = dto.FoodName;
        entity.Description = dto.Description;
        entity.Price = dto.Price;
        entity.ImageURL = dto.ImgUrl;
        entity.StatusID = dto.StatusID;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.CategoryID = dto.CategoryId;

        // 4. Cập nhật và lưu
        _unitOfWork.FoodItems.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        // 5. Trả về true (thành công)
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _unitOfWork.FoodItems.GetByIdAsync(id);
        if (entity == null)
            return false;

        _unitOfWork.FoodItems.Remove(entity);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    // Hàm helper (trợ giúp) để tránh lặp code mapping
    private FoodItemDTO MapToDTO(FoodItem f)
    {
        return new FoodItemDTO
        {
            FoodId = f.FoodID,
            FoodName = f.FoodName,
            Description = f.Description,
            Price = f.Price,
            ImgUrl = f.ImageURL,
            StatusID = f.StatusID,
            StatusName = f.FoodStatus?.StatusName,
            CreatedAt = f.CreatedAt,
            UpdatedAt = f.UpdatedAt,
            CategoryId = f.CategoryID,
            CategoryName = f.Category?.Name
        };
    }
}