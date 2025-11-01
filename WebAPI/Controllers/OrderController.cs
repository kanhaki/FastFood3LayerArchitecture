using BUS.Services;
using DTO.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _service.GetByIdAsync(id);
            if (order == null)
                return NotFound(new { message = $"Không tìm thấy order với ID: {id}" });
            return Ok(order);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] OrderDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            if (!int.TryParse(userIdString, out int userIdFromToken))
            {
                // Token bị "lỗi"
                return BadRequest(new { message = "Token chứa UserID không hợp lệ." });
            }

            try
            {
                var responseDto = await _service.CreateAsync(dto, userIdFromToken);

                // Trả về 201 Created (với URL trỏ đến API)
                return CreatedAtAction(nameof(GetById),
                                    new { id = responseDto.OrderID },
                                    responseDto);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { message = "Tạo order thất bại. " + ex.Message });
            }
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequestDTO dto)
        {
            var result = await _service.UpdateStatusAsync(id, dto.Status);

            switch (result)
            {
                case UpdateStatusResult.Success:
                    return NoContent();

                case UpdateStatusResult.OrderNotFound:
                    return NotFound(new { message = $"Không tìm thấy order với ID: {id}" });

                case UpdateStatusResult.StatusNotFound:
                    return BadRequest(new { message = $"Trạng thái '{dto.Status}' không hợp lệ." });

                default:
                    return StatusCode(500, "Lỗi máy chủ không xác định");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = $"Không tìm thấy order với ID: {id} để xóa" });

            // SỬA: Trả về 204 NoContent
            return NoContent();
        }
    }
}
