using DAT.Entity;
using DAT.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IGenericRepository<Category> Categories { get; }
        IGenericRepository<FoodItem> FoodItems { get; }
        IGenericRepository<T> Repository<T>() where T : class;
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
