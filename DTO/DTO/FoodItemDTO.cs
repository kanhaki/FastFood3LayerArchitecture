using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO
{
    public class FoodItemDTO
    {
        public long FoodId { get; set; }
        public string FoodName { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }
        public string? ImgUrl { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long CategoryId { get; set; }
        public string? CategoryName { get; set; } // tiện để hiển thị
    }
}
