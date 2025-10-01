using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAT.Entity
{
    public class User
    {
        [Key]
        public long UserId { get; set; }

        [Required, MaxLength(255)]
        public string FullName { get; set; }

        [Required, MaxLength(255)]
        public string Email { get; set; }

        [Required, MaxLength(255)]
        public string PasswordHash { get; set; }

        public DateOnly? DOB { get; set; }

        [Required, MaxLength(20)]
        public string Role { get; set; } // enum: customer, admin, staff

        public string AvatarUrl { get; set; }
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation
        public ICollection<Address> Addresses { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<BlogPost> BlogPosts { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
