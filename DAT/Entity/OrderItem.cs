using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Entity
{
    public class OrderItem
    {
        [Key]
        public long OrderItemId { get; set; }

        public long OrderId { get; set; }
        public Order Order { get; set; }

        public long FoodId { get; set; }
        public FoodItem FoodItem { get; set; }

        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
    }
}
