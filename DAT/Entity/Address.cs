using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Entity
{
    public class Address
    {
        [Key]
        public long AddrId { get; set; }

        [Required, MaxLength(255)]
        public string AddressName { get; set; }

        public string AddressLine { get; set; }
        public string Phone { get; set; }

        public bool IsDefault { get; set; }

        // FK
        public long UserId { get; set; }
        public User User { get; set; }
    }
}
