using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Entity
{
    public class Review
    {
        [Key]
        public long ReviewId { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }

        public string Content { get; set; }
        public int Rating { get; set; }
        public bool IsApproved { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
