using DAT.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Repository
{
    public interface IFoodItemRepository : IGenericRepository<FoodItem>
    {
        // Hàm này sẽ lấy FoodItem KÈM Category và FoodStatus
        Task<IEnumerable<FoodItem>> GetAllWithDetailsAsync(CancellationToken ct = default);

        // Hàm này lấy 1 FoodItem KÈM Category và FoodStatus
        Task<FoodItem?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default);

        // Hàm này lấy FoodItem theo Category (cũng KÈM details)
        Task<IEnumerable<FoodItem>> GetByCategoryWithDetailsAsync(int categoryId, CancellationToken ct = default);
    }
}
