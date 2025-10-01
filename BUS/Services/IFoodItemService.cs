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
        Task<FoodItemDTO?> GetByIdAsync(long id);
        Task AddAsync(FoodItemDTO dto);
        Task UpdateAsync(FoodItemDTO dto);
        Task<bool> DeleteAsync(long id);
    }

}
