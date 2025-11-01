using BUS.Services.PaymentService;
using DAT.Entity;
using DAT.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("payments")]
    public class PaymentController : ControllerBase
    {
        // SỬA: "Tiêm" IVnPayService (mới)
        private readonly IVnPayService _vnPayService;
        private readonly IUnitOfWork _uow; // (Chúng ta vẫn cần UoW để lưu CSDL)

        public PaymentController(IVnPayService vnPayService, IUnitOfWork uow)
        {
            _vnPayService = vnPayService;
            _uow = uow;
        }

        // SỬA: Đây là 'ReturnUrl' (cho User)
        [HttpGet("vnpay-return")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> VnpayReturn()
        {
            var vnpayParams = HttpContext.Request.Query;

            // "Dùng" Service mới
            var response = _vnPayService.PaymentExecute(vnpayParams);

            if (response == null || !response.Success)
            {
                // (Thanh toán thất bại -> Redirect về trang "Thất bại")
                return Redirect("http://localhost:3000/payment-failed");
            }

            // --- QUAN TRỌNG: CẬP NHẬT CSDL ---
            // "Docs" quên mất bước này!
            // (Chúng ta sẽ "trộm" logic của IPaymentService cũ)
            var order = await _uow.Orders.GetByIdAsync(int.Parse(response.OrderId));
            var orderStatus = (await _uow.Repository<OrderStatus>().FindAsync(s => s.StatusName == "Confirmed")).First();
            var paymentStatus = (await _uow.Repository<PaymentTransactionStatus>().FindAsync(s => s.StatusName == "Success")).First();

            order.StatusID = orderStatus.StatusID;
            _uow.Orders.Update(order);

            var newTransaction = new PaymentTransaction
            {
                vnp_TransactionNo = response.TransactionId,
                OrderID = order.OrderID,
                OrderInfo = response.OrderDescription,
                StatusID = paymentStatus.StatusID,
                PaymentDate = DateTime.UtcNow,
                BankCode = "VNPAY", // (ReturnUrl ko có bank code)
                ResponseCode = response.VnPayResponseCode,
                Amount = order.TotalAmount
            };
            await _uow.Repository<PaymentTransaction>().AddAsync(newTransaction);
            await _uow.SaveChangesAsync();

            // Trả về trang "Cảm ơn" của Front-end
            return Redirect($"http://localhost:3000/thank-you?orderId={response.OrderId}");
        }
    }
}
