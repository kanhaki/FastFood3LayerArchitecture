using DAT.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Repository
{
    public interface IComboRepository
    {
        Task<Product> GetComboWithDetailsAsync(int comboId);
    }

    public class ComboRepository : IComboRepository
    {
        private readonly FastFoodDbContext _context;
        public ComboRepository(FastFoodDbContext context)
        {
            _context = context;
        }

        public async Task<Product> GetComboWithDetailsAsync(int comboId)
        {
            return await _context.Products
                .Include(p => p.ComboDetails)
                    .ThenInclude(cd => cd.ItemProduct)
                .Include(p => p.ComboOptionGroups)
                    .ThenInclude(cog => cog.ComboOptionItems)
                        .ThenInclude(coi => coi.Product)
                .FirstOrDefaultAsync(p => p.ProductId == comboId && p.IsCombo);
        }
    }
}
