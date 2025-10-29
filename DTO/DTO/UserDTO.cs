using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO
{
    public class UserDTO
    {
        public int UserID { get; set; } // SỬA: UserId -> UserID
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime? DOB { get; set; }

        public int RoleID { get; set; } // THÊM: Dùng để GHI
        public string RoleName { get; set; } // SỬA: Dùng để ĐỌC

        public string AvatarURL { get; set; } // SỬA: AvatarUrl -> AvatarURL
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // THÊM: Dùng cho AddUserAsync (thay vì lưu "123456")
        public string Password { get; set; }
    }
}
