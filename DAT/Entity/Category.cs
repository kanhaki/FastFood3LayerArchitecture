using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Entity
{
    public class Category
    {
        [Key]
        public long CategoryId { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; }

        public string Description { get; set; }
        public string ImgUrl { get; set; }

        // Navigation
        public ICollection<FoodItem> FoodItems { get; set; }
    }
}
