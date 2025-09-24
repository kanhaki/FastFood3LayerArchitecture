using DAT.Entity;
using DAT.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IComboRepository _comboRepository;

        public ProductService(IProductRepository productRepository, IComboRepository comboRepository)
        {
            _productRepository = productRepository;
            _comboRepository = comboRepository;
        }

        public async Task<List<Product>> GetAllProductsAsync()
            => await _productRepository.GetAllProductsAsync();

        public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId, bool isCombo)
            => await _productRepository.GetProductsByCategoryAsync(categoryId, isCombo);

        public async Task<List<Product>> GetComboProductsAsync()
            => await _productRepository.GetComboProductsAsync();

        public async Task<Product> GetComboWithDetailsAsync(int comboId)
            => await _comboRepository.GetComboWithDetailsAsync(comboId);
    }
}
