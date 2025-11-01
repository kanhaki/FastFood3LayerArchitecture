using DAT.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Repository
{
    public interface IRestaurantRepository : IGenericRepository<Restaurant>
    {
        Task<IEnumerable<Restaurant>> GetAllWithStatusAsync();
        Task<Restaurant?> GetByIdWithStatusAsync(int id);
    }
}
