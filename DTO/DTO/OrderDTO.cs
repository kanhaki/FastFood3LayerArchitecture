using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO
{
    public class OrderDTO
    {
        public long OrderId { get; set; }
        public long UserId { get; set; }
        public long AdrsId { get; set; }
        public DateTime OrderTime { get; set; }
        public string Status { get; set; } = "Pending";
        public int TotalAmount { get; set; }
        public long? PromoId { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<OrderItemDTO> Items { get; set; } = new();
    }

}
