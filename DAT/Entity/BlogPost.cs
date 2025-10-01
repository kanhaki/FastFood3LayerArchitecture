using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Entity
{
    public class BlogPost
    {
        [Key]
        public long BlogPostId { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }

        [Required, MaxLength(255)]
        public string Title { get; set; }

        public string Content { get; set; }
        public bool IsApproved { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation
        public ICollection<BlogPostImage> BlogPostImages { get; set; }
    }
}
