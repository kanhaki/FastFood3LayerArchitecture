using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Entity
{
    public class Product
    {
        public int ProductId { get; set; }
        public string SKU { get; set; }
        public int CategoryId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public bool IsCombo { get; set; }
        public string? ImageUrl { get; set; }

        public Category Category { get; set; }
        public ICollection<ComboDetail> ComboDetails { get; set; }
        public ICollection<ComboOptionGroup> ComboOptionGroups { get; set; }
    }
}
