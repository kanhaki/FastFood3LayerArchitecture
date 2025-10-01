using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Entity
{
    public class Reservation
    {
        [Key]
        public long ReservationId { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }

        public DateTime ReservationTime { get; set; }
        public int NumGuests { get; set; }

        public string Status { get; set; } // pending, confirmed, cancelled

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
