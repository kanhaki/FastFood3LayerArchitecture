using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Entity
{
    public class BlogPostImage
    {
        [Key]
        public long ImageId { get; set; }

        public long BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public int SortOrder { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
