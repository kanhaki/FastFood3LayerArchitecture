using DAT.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<List<Product>> GetProductsByCategoryAsync(int categoryId, bool isCombo);
        Task<List<Product>> GetComboProductsAsync();
        Task<Product> GetComboWithDetailsAsync(int comboId);
    }
}
