using AutoMapper;
using DAT.Entity;
using DAT.Repository;
using DTO.DTO;
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
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        // Lấy tất cả sản phẩm
        public async Task<List<ProductDTO>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return _mapper.Map<List<ProductDTO>>(products);
        }

        // Lấy sản phẩm theo Category
        public async Task<List<ProductDTO>> GetProductsByCategoryAsync(int categoryId, bool isCombo)
        {
            var products = await _productRepository.GetProductsByCategoryAsync(categoryId, isCombo);
            return _mapper.Map<List<ProductDTO>>(products);
        }

        // Lấy combo với chi tiết
        public async Task<ProductDTO> GetComboWithDetailsAsync(int comboId)
        {
            var combo = await _productRepository.GetProductByIdAsync(comboId);
            return _mapper.Map<ProductDTO>(combo);
        }

        public async Task<ProductDTO> GetProductByIdAsync(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            return _mapper.Map<ProductDTO>(product);
        }
    }

}
