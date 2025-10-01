using BUS.Services;
using DTO.DTO;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoodItemController : ControllerBase
    {
        private readonly IFoodItemService _service;

        public FoodItemController(IFoodItemService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var f = await _service.GetByIdAsync(id);
            if (f == null) return NotFound();
            return Ok(f);
        }

        [HttpPost]
        public async Task<IActionResult> Add(FoodItemDTO dto)
        {
            await _service.AddAsync(dto);
            return Ok(new { message = "Food item created" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, FoodItemDTO dto)
        {
            if (id != dto.FoodId) return BadRequest();
            await _service.UpdateAsync(dto);
            return Ok(new { message = "Food item updated" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return NotFound();
            return Ok(new { message = "Food item deleted" });
        }
    }

}
