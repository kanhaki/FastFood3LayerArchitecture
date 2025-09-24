using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Entity
{
    public class ComboDetail
    {
        public int ComboDetailId { get; set; }
        public int ComboProductId { get; set; }
        public int ItemProductId { get; set; }
        public int Quantity { get; set; }

        public Product ComboProduct { get; set; }
        public Product ItemProduct { get; set; }
    }
}
