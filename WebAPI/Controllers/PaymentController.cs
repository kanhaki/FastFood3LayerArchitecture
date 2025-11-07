using BUS.Services.PaymentService;
using DAT.Entity;
using DAT.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("payments")]
    public class PaymentController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;
        private readonly IUnitOfWork _uow;

        public PaymentController(IVnPayService vnPayService, IUnitOfWork uow)
        {
            _vnPayService = vnPayService;
            _uow = uow;
        }

        [HttpGet("vnpay-return")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> VnpayReturn()
        {
            var vnpayParams = HttpContext.Request.Query;

            var response = _vnPayService.PaymentExecute(vnpayParams);

            if (response == null || !response.Success)
            {
                // (Chữ ký thất bại -> Redirect về trang "Thất bại")
                return Redirect("http://localhost:3000/payment-failed");
            }

            // Chữ ký OK -> Trả về trang "Cảm ơn" của Front-end
            // Front-end sẽ tự gọi API khác để check trạng thái đơn hàng (đã được IPN cập nhật)
            return Redirect($"http://localhost:3000/thank-you?orderId={response.OrderId}");
        }

        [HttpGet("vnpay-ipn")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> VnpayIpnReturn()
        {
            var vnpayParams = HttpContext.Request.Query;

            // Dùng Service để check chữ ký
            var response = _vnPayService.PaymentExecute(vnpayParams);

            // 1. KIỂM TRA CHỮ KÝ
            if (response == null || !response.Success)
            {
                // Chữ ký không hợp lệ
                return Content(JsonSerializer.Serialize(new { RspCode = "97", Message = "Invalid signature" }), "application/json");
            }

            // 2. KIỂM TRA LOGIC NGHIỆP VỤ
            var order = await _uow.Orders.GetByIdAsync(int.Parse(response.OrderId));

            // 2a. Check Order có tồn tại?
            if (order == null)
            {
                return Content(JsonSerializer.Serialize(new { RspCode = "01", Message = "Order not found" }), "application/json");
            }

            // 2b. Check số tiền
            var vnpAmount = Convert.ToInt64(vnpayParams.FirstOrDefault(k => k.Key == "vnp_Amount").Value) / 100;
            if (order.TotalAmount != vnpAmount)
            {
                return Content(JsonSerializer.Serialize(new { RspCode = "04", Message = "Invalid amount" }), "application/json");
            }

            // 2c. Check trạng thái đơn hàng (Tránh xử lý lại)
            var pendingStatus = (await _uow.Repository<OrderStatus>().FindAsync(s => s.StatusName == "Pending")).FirstOrDefault();
            // Nếu đơn hàng KHÔNG còn là "Pending" -> Nó đã được xử lý rồi
            if (pendingStatus == null)
            {
                // Đây là lỗi nghiêm trọng (chưa seed CSDL)
                return Content(JsonSerializer.Serialize(new { RspCode = "99", Message = "Database Error: Pending status not found" }), "application/json");
            }

            if (order.StatusID != pendingStatus.StatusID)
            {
                return Content(JsonSerializer.Serialize(new { RspCode = "02", Message = "Order already confirmed" }), "application/json");
            }

            // 3. CẬP NHẬT CSDL
            var paymentStatus = (await _uow.Repository<PaymentTransactionStatus>().FindAsync(s => s.StatusName == "Success")).FirstOrDefault();

            var newTransaction = new PaymentTransaction
            {
                vnp_TransactionNo = response.TransactionId,
                OrderID = order.OrderID,
                OrderInfo = response.OrderDescription,
                StatusID = paymentStatus.StatusID,
                PaymentDate = DateTime.UtcNow,
                BankCode = vnpayParams.FirstOrDefault(k => k.Key == "vnp_BankCode").Value,
                ResponseCode = response.VnPayResponseCode,
                Amount = order.TotalAmount
            };
            await _uow.Repository<PaymentTransaction>().AddAsync(newTransaction);

            await _uow.SaveChangesAsync();

            // 4. TRẢ VỀ "CÔNG VĂN" CHO VNPAY
            // Phải là mã 00 thì VNPay mới ngừng gọi
            return Content(JsonSerializer.Serialize(new { RspCode = "00", Message = "Confirm Success" }), "application/json");
        }
    }
}