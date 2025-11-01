using DTO.DTO.Payment;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<VnpayIpnResponse> ProcessVnpayIpnAsync(IQueryCollection vnpayParams);
    }
}
