using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO.Payment
{
    public class VnpayIpnResponse
    {
        public string VnpayResponseJson { get; set; }
        public string LogMessage { get; set; }
        public bool IsSuccess { get; set; }
    }
}
