using BUS.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId, bool isCombo = false)
        {
            var products = await _productService.GetProductsByCategoryAsync(categoryId, isCombo);
            return Ok(products);
        }

        [HttpGet("combo/{comboId}")]
        public async Task<IActionResult> GetComboDetails(int comboId)
        {
            var combo = await _productService.GetComboWithDetailsAsync(comboId);
            return Ok(combo);
        }
    }

}
