using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Entity
{
    public class Promotion
    {
        [Key]
        public long PromoId { get; set; }

        [Required, MaxLength(255)]
        public string PromoName { get; set; }

        public string Description { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal MinOrderAmount { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Navigation
        public ICollection<FoodItemPromotion> FoodItemPromotions { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
