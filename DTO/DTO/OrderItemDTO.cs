using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO
{
    public class OrderItemDTO
    {
        public int FoodID { get; set; } // SỬA: long -> int, FoodId -> FoodID
        public int Quantity { get; set; }
        public int Price { get; set; }
    }
}
