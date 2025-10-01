using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Entity
{
    public class FoodItem
    {
        [Key]
        public long FoodId { get; set; }

        [Required, MaxLength(255)]
        public string FoodName { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }
        public string ImgUrl { get; set; }

        public bool Status { get; set; } = true;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // FK
        public long CategoryId { get; set; }
        public Category Category { get; set; }

        // Navigation
        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<FoodItemPromotion> FoodItemPromotions { get; set; }
    }
}
