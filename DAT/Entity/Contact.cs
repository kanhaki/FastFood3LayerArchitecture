using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Entity
{
    public class Contact
    {
        [Key]
        public long ContactId { get; set; }

        [Required, MaxLength(255)]
        public string ContactName { get; set; }

        [Required, MaxLength(255)]
        public string Email { get; set; }

        public string Message { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
