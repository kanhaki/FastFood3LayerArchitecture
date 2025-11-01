using DAT.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Repository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
        Task<User?> GetByIdWithAddressesAsync(int id, CancellationToken ct = default); // example include
        Task<IEnumerable<User>> GetAllWithRolesAsync(CancellationToken ct = default);
        Task<User?> GetByIdWithRoleAsync(int id, CancellationToken ct = default);
    }
}
