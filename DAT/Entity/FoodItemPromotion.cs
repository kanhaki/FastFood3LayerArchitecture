using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Entity
{
    public class FoodItemPromotion
    {
        public long FoodId { get; set; }
        public FoodItem FoodItem { get; set; }

        public long PromoId { get; set; }
        public Promotion Promotion { get; set; }
    }
}
