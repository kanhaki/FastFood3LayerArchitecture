using DAT.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DAT.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
            => await _dbSet.SingleOrDefaultAsync(u => u.Email == email, ct);

        public async Task<User?> GetByIdWithAddressesAsync(long id, CancellationToken ct = default)
            => await _dbSet.Include(u => u.Addresses) // giả sử User có nav property Addresses
                       .SingleOrDefaultAsync(u => u.UserId == id, ct);
    }
}
