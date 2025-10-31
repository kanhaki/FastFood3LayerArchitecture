using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Services
{
    public interface IFoodItemService
    {
        Task<IEnumerable<FoodItemDTO>> GetAllAsync();
        Task<IEnumerable<FoodItemDTO>> GetFoodsByCategoryAsync(int categoryId);
        Task<FoodItemDTO?> GetByIdAsync(int id);
        Task AddAsync(FoodItemDTO dto);
        Task<bool> UpdateAsync(FoodItemDTO dto);
        Task<bool> DeleteAsync(int id);
    }

}
