using DAT.Entity;
using DAT.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly Dictionary<Type, object> _repos = new();
        private IUserRepository? _userRepo;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IUserRepository Users => _userRepo ??= new UserRepository(_context);
        public IGenericRepository<Category> Categories => Repository<Category>();
        public IGenericRepository<FoodItem> FoodItems => Repository<FoodItem>();
        public IGenericRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);
            if (_repos.ContainsKey(type)) return (IGenericRepository<T>)_repos[type]!;
            var repo = new GenericRepository<T>(_context);
            _repos[type] = repo;
            return repo;
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default) => _context.SaveChangesAsync(ct);

        public void Dispose() => _context.Dispose();
    }
}
