using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO.Order
{
    public class OrderCreationResponseDTO
    {
        public int OrderID { get; set; }
        public string PaymentUrl { get; set; }
    }
}
