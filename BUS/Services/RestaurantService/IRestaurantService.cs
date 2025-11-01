using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Services.RestaurantService
{
    public interface IRestaurantService
    {
        Task<IEnumerable<RestaurantDTO>> GetAllAsync();
        Task<RestaurantDTO?> GetByIdAsync(int id);

        // (Tạm thời chúng ta chưa cần Add/Update/Delete nhà hàng, 
        // vì đó là việc của Admin, làm sau)
    }
}
