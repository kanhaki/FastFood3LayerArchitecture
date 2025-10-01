using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO
{
    public class UserDTO
    {
        public long UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateOnly? DOB { get; set; }
        public string Role { get; set; } // enum: customer, admin, staff
        public string AvatarUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
