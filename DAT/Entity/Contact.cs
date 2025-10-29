using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Entity
{
    [Table("Contact")]
    public class Contact
    {
        [Key]
        public int ContactID { get; set; }

        [StringLength(255)]
        public string ContactName { get; set; }

        [Required]
        [StringLength(255)]
        public string Email { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
