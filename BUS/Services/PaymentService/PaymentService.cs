using Common;
using DAT.Entity;
using DAT.UnitOfWork;
using DTO.DTO.Payment;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _config;

        public PaymentService(IUnitOfWork unitOfWork, IConfiguration config)
        {
            _uow = unitOfWork;
            _config = config;
        }

        public async Task<VnpayIpnResponse> ProcessVnpayIpnAsync(IQueryCollection vnpayParams)
        {
            const string RspCodeSuccess = "00";
            const string RspCodeFail = "99"; // Bất kỳ mã nào khác "00"
            const string VnpayResponseSuccess = "{\"RspCode\":\"00\",\"Message\":\"Confirm Success\"}";
            const string VnpayResponseFail = "{\"RspCode\":\"99\",\"Message\":\"Confirm Failed\"}";

            try
            {
                // 1. Lấy "chìa khóa bí mật" (HashSecret) từ appsettings.json
                string hashSecret = _config["Vnpay:HashSecret"];
                if (string.IsNullOrEmpty(hashSecret))
                {
                    return new VnpayIpnResponse { IsSuccess = false, LogMessage = "VNPAY HASH SECRET NOT CONFIGURED", VnpayResponseJson = VnpayResponseFail };
                }

                // 2. Xác thực "chữ ký"
                bool isSignatureValid = VnpayHelper.ValidateSignature(vnpayParams, hashSecret);
                if (!isSignatureValid)
                {
                    return new VnpayIpnResponse { IsSuccess = false, LogMessage = "Checksum failed (Chữ ký không hợp lệ)", VnpayResponseJson = VnpayResponseFail };
                }

                // 3. Lấy các thông số
                string vnp_ResponseCode = vnpayParams["vnp_ResponseCode"];
                string vnp_TxnRef = vnpayParams["vnp_TxnRef"]; // Đây là OrderID của chúng ta
                string vnp_TransactionNo = vnpayParams["vnp_TransactionNo"]; // Mã của VNPAY
                int vnp_Amount = Convert.ToInt32(vnpayParams["vnp_Amount"]) / 100; // VNPAY gửi (Amount * 100), ta phải chia lại

                if (!int.TryParse(vnp_TxnRef, out int orderId))
                {
                    return new VnpayIpnResponse { IsSuccess = false, LogMessage = $"Invalid vnp_TxnRef (OrderID): {vnp_TxnRef}", VnpayResponseJson = VnpayResponseFail };
                }

                // 4. Kiểm tra CSDL
                var order = await _uow.Orders.GetByIdAsync(orderId);
                if (order == null)
                {
                    return new VnpayIpnResponse { IsSuccess = false, LogMessage = $"Order {orderId} not found", VnpayResponseJson = VnpayResponseFail };
                }

                // 5. Kiểm tra xem đã xử lý giao dịch này chưa
                var existingTransaction = (await _uow.Repository<PaymentTransaction>()
                    .FindAsync(t => t.vnp_TransactionNo == vnp_TransactionNo))
                    .FirstOrDefault();

                if (existingTransaction != null && existingTransaction.StatusID == 1) // 1 = Success
                {
                    // Đã xử lý thành công rồi -> Trả về Success (tránh VNPAY gọi lại)
                    return new VnpayIpnResponse { IsSuccess = true, LogMessage = $"Transaction {vnp_TransactionNo} already processed.", VnpayResponseJson = VnpayResponseSuccess };
                }

                // Lấy các bảng "lookup"
                var paymentStatusSuccess = (await _uow.Repository<PaymentTransactionStatus>().FindAsync(s => s.StatusName == "Success")).First();
                var paymentStatusFailed = (await _uow.Repository<PaymentTransactionStatus>().FindAsync(s => s.StatusName == "Failed")).First();
                var orderStatusConfirmed = (await _uow.Repository<OrderStatus>().FindAsync(s => s.StatusName == "Confirmed")).First();
                var orderStatusCancelled = (await _uow.Repository<OrderStatus>().FindAsync(s => s.StatusName == "Cancelled")).First();

                // 6. Xử lý logic thanh toán
                if (vnp_ResponseCode == RspCodeSuccess)
                {
                    // THÀNH CÔNG
                    order.StatusID = orderStatusConfirmed.StatusID; // Chuyển "Confirmed"
                    _uow.Orders.Update(order);

                    var newTransaction = new PaymentTransaction
                    {
                        vnp_TransactionNo = vnp_TransactionNo,
                        OrderID = orderId,
                        OrderInfo = vnpayParams["vnp_OrderInfo"],
                        StatusID = paymentStatusSuccess.StatusID, // "Success"
                        PaymentDate = DateTime.UtcNow,
                        BankCode = vnpayParams["vnp_BankCode"],
                        ResponseCode = vnp_ResponseCode,
                        Amount = vnp_Amount
                    };
                    await _uow.Repository<PaymentTransaction>().AddAsync(newTransaction);

                    await _uow.SaveChangesAsync();
                    return new VnpayIpnResponse { IsSuccess = true, LogMessage = $"Payment success for Order {orderId}", VnpayResponseJson = VnpayResponseSuccess };
                }
                else
                {
                    // THẤT BẠI (ví dụ: User hủy)
                    order.StatusID = orderStatusCancelled.StatusID; // Chuyển "Cancelled"
                    _uow.Orders.Update(order);

                    // (Bạn có thể chọn lưu hoặc không lưu giao dịch thất bại)

                    await _uow.SaveChangesAsync();
                    return new VnpayIpnResponse { IsSuccess = false, LogMessage = $"Payment failed for Order {orderId}. Code: {vnp_ResponseCode}", VnpayResponseJson = VnpayResponseFail };
                }
            }
            catch (Exception ex)
            {
                // (Ghi log 'ex' lại)
                return new VnpayIpnResponse { IsSuccess = false, LogMessage = $"UNKNOWN EXCEPTION: {ex.Message}", VnpayResponseJson = VnpayResponseFail };
            }
        }
    }
}
