using BUS.Services;
using DTO.DTO;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("foods")]
    public class FoodItemController : ControllerBase
    {
        private readonly IFoodItemService _service;

        public FoodItemController(IFoodItemService service)
        {
            _service = service;
        }

        // GET /foods/getAllFoods
        [HttpGet("getAllFoods")]
        public async Task<IActionResult> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        // GET /foods/getFoodsByCategory/{categoryId}
        [HttpGet("getFoodsByCategory/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            var items = await _service.GetFoodsByCategoryAsync(categoryId);
            if (!items.Any())
                return NotFound(new { error = "Không tìm thấy món ăn trong category này", code = 404 });

            return Ok(items);
        }

        // GET /foods/getFoodInfo/{id}
        [HttpGet("getFoodInfo/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null)
                return NotFound(new { error = "Không tìm thấy món ăn", code = 404 });
            return Ok(item);
        }

        // POST /foods/createFood
        [HttpPost("createFood")]
        public async Task<IActionResult> Create([FromBody] FoodItemDTO dto)
        {
            await _service.AddAsync(dto);
            return Ok(new { message = "Tạo món ăn thành công", foodName = dto.FoodName });
        }

        // PUT /foods/updateFood/{id}
        [HttpPut("updateFood/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] FoodItemDTO dto)
        {
            if (id != dto.FoodId)
                return BadRequest(new { error = "ID không khớp với dữ liệu gửi lên", code = 400 });

            try
            {
                await _service.UpdateAsync(dto);
                return Ok(new { message = "Cập nhật thành công", foodId = dto.FoodId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Không thể cập nhật món ăn", detail = ex.Message });
            }
        }

        // DELETE /foods/deleteFood/{id}
        [HttpDelete("deleteFood/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return NotFound(new { error = "Không tìm thấy món ăn để xóa", code = 404 });

            return Ok(new { message = "Xóa món ăn thành công", foodId = id });
        }
    }
}
