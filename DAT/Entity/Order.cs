using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Entity
{
    public class Order
    {
        [Key]
        public long OrderId { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }

        public long AddrId { get; set; }
        public Address Address { get; set; }

        public DateTime OrderTime { get; set; }
        public decimal TotalAmount { get; set; }

        public string Status { get; set; } // pending, confirmed, shipped, delivered, cancelled

        public long? PromoId { get; set; }
        public Promotion Promotion { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
