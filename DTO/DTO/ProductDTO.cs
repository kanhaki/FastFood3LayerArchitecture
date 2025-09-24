using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string SKU { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public bool IsCombo { get; set; }
        public string ImageUrl { get; set; }

        public List<ComboOptionGroupDTO> ComboOptionGroups { get; set; } = new List<ComboOptionGroupDTO>();
    }
}
