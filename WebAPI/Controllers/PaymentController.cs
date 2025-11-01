using BUS.Services.PaymentService;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("payments")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // LƯU Ý: VNPAY IPN gọi bằng [HttpGet]
        [HttpGet("vnpay-callback")]
        [ApiExplorerSettings(IgnoreApi = true)] // <-- Ẩn endpoint này khỏi Swagger (vì nó cho máy)
        public async Task<IActionResult> VnpayCallback()
        {
            var vnpayParams = HttpContext.Request.Query;

            var result = await _paymentService.ProcessVnpayIpnAsync(vnpayParams);

            // Ghi log lại (cực kỳ quan trọng để debug)
            Console.WriteLine($"VNPAY IPN Response: {result.LogMessage}");

            // Trả về "lời thề" (JSON) mà VNPAY bắt buộc
            return Content(result.VnpayResponseJson, "application/json");
        }
    }
}
