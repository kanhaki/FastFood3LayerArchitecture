using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO
{
    public class OrderItemDTO
    {
        public long OrderItemId { get; set; }
        public long FoodId { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
    }

}
