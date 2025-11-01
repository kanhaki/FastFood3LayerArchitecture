using DAT.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Repository
{
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        public AddressRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Address>> GetAllForUserAsync(int userId)
        {
            // Bảo mật: Luôn lọc theo UserID
            return await _dbSet
                        .Where(a => a.UserID == userId)
                        .ToListAsync();
        }

        public async Task<Address?> GetByIdForUserAsync(int addressId, int userId)
        {
            // Bảo mật: Lọc theo CẢ UserID VÀ AddressID
            return await _dbSet
                        .SingleOrDefaultAsync(a => a.AdrsID == addressId && a.UserID == userId);
        }
    }
}
