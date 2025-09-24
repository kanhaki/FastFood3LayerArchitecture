using DAT.Entity;
using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Services
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetAllProductsAsync();
        Task<List<ProductDTO>> GetProductsByCategoryAsync(int categoryId, bool isCombo);
        Task<List<ProductDTO>> GetComboProductsAsync();
        Task<ProductDTO> GetComboWithDetailsAsync(int comboId);
        Task<ProductDTO> GetProductByIdAsync(int productId);
    }
}
