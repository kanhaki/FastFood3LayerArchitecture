using BUS.Services;
using DTO.DTO;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var c = await _service.GetByIdAsync(id);
            if (c == null) return NotFound();
            return Ok(c);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CategoryDTO dto)
        {
            await _service.AddAsync(dto);
            return Ok(new { message = "Category created" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, CategoryDTO dto)
        {
            if (id != dto.CategoryId) return BadRequest();
            await _service.UpdateAsync(dto);
            return Ok(new { message = "Category updated" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return NotFound();
            return Ok(new { message = "Category deleted" });
        }
    }

}
