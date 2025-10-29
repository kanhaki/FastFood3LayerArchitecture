using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Entity
{
    [Table("OrderItem")]
    public class OrderItem
    {
        // Đây là KHÓA CHÍNH PHỨC HỢP (Composite Key)
        // Bạn phải định nghĩa nó trong AppDbContext dùng Fluent API
        // .HasKey(oi => new { oi.OrderID, oi.FoodID });

        public int OrderID { get; set; }
        public int FoodID { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int Price { get; set; } // Giá tại thời điểm mua

        // Navigation properties
        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }

        [ForeignKey("FoodID")]
        public virtual FoodItem FoodItem { get; set; }
    }
}
