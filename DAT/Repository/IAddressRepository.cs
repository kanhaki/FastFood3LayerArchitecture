using DAT.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Repository
{
    public interface IAddressRepository : IGenericRepository<Address>
    {
        Task<IEnumerable<Address>> GetAllForUserAsync(int userId);
        Task<Address?> GetByIdForUserAsync(int addressId, int userId);
    }
}
