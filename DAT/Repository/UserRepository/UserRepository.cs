using DAT.Entity;
using Microsoft.EntityFrameworkCore; // <--- Đảm bảo bạn đã using cái này
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        {

            return await _dbSet
                        .Include(u => u.UserRole)
                        .SingleOrDefaultAsync(u => u.Email == email, ct);
        }

        public async Task<User?> GetByIdWithAddressesAsync(int id, CancellationToken ct = default)
        {
            return await _dbSet.Include(u => u.Addresses)
                             .SingleOrDefaultAsync(u => u.UserID == id, ct);
        }
        public async Task<IEnumerable<User>> GetAllWithRolesAsync(CancellationToken ct = default)
        {
            return await _dbSet
                        .Include(u => u.UserRole)
                        .ToListAsync(ct);
        }

        public async Task<User?> GetByIdWithRoleAsync(int id, CancellationToken ct = default)
        {
            return await _dbSet
                        .Include(u => u.UserRole)
                        .SingleOrDefaultAsync(u => u.UserID == id, ct);
        }
    }
}