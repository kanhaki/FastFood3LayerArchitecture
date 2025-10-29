using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Entity
{
    [Table("Order")] // Bắt buộc dùng [Table] vì "Order" là từ khóa SQL
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        public int UserID { get; set; }
        public int AdrsID { get; set; }
        public int RestaurantID { get; set; }
        public int StatusID { get; set; }

        [Required]
        public DateTime OrderTime { get; set; }

        [Required]
        public int TotalAmount { get; set; } // Đã là INT

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [ForeignKey("AdrsID")]
        public virtual Address Address { get; set; }

        [ForeignKey("RestaurantID")]
        public virtual Restaurant Restaurant { get; set; }

        [ForeignKey("StatusID")]
        public virtual OrderStatus OrderStatus { get; set; }

        // Navigation property (cho quan hệ Many-to-Many)
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        // Navigation property (cho quan hệ One-to-One)
        public virtual PaymentTransaction PaymentTransaction { get; set; }

        // Navigation property (cho quan hệ One-to-One)
        public virtual Delivery Delivery { get; set; }
    }
}
