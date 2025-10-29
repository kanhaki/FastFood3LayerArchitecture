using DAT.UnitOfWork;
using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAT.Entity; // <-- Đảm bảo using
using Microsoft.EntityFrameworkCore; // <-- Đảm bảo using

namespace BUS.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            
            var users = await _unitOfWork.Users.GetAllWithRolesAsync();

            return users.Select(u => new UserDTO
            {
                UserID = u.UserID,           // SỬA: UserId -> UserID
                FullName = u.FullName,
                Email = u.Email,
                DOB = u.DOB,
                RoleID = u.RoleID,           // SỬA: Thêm RoleID
                RoleName = u.UserRole?.RoleName, // SỬA: Role -> UserRole.RoleName
                AvatarURL = u.AvatarURL,       // SỬA: AvatarUrl -> AvatarURL
                // IsActive = u.IsActive,    // SỬA: Đã xóa
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt,
            });
        }

        public async Task<UserDTO?> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdWithRoleAsync(id);

            if (user == null) return null;

            return new UserDTO
            {
                UserID = user.UserID,           // SỬA
                FullName = user.FullName,
                Email = user.Email,
                DOB = user.DOB,
                RoleID = user.RoleID,           // SỬA
                RoleName = user.UserRole?.RoleName, // SỬA
                AvatarURL = user.AvatarURL,       // SỬA
                // IsActive = user.IsActive,    // SỬA
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
            };
        }

        public async Task AddUserAsync(UserDTO userDto)
        {
            // SỬA LỖI LOGIC: Phải tìm RoleID từ RoleName
            var roleName = string.IsNullOrEmpty(userDto.RoleName) ? "customer" : userDto.RoleName;
            var role = (await _unitOfWork.UserRoles.FindAsync(r => r.RoleName == roleName)).FirstOrDefault();

            if (role == null)
                throw new Exception($"Role '{roleName}' not found.");

            // SỬA LỖ HỔNG BẢO MẬT: Không bao giờ lưu plaintext "123456"
            // Phải HASH password.
            if (string.IsNullOrWhiteSpace(userDto.Password)) // Giả sử DTO có Password
                throw new ArgumentException("Password is required to create a user.");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            var user = new DAT.Entity.User
            {
                FullName = userDto.FullName,
                Email = userDto.Email,
                PasswordHash = passwordHash,      // SỬA
                DOB = userDto.DOB,
                RoleID = role.RoleID,             // SỬA
                AvatarURL = userDto.AvatarURL,      // SỬA
                // IsActive = userDto.IsActive,  // SỬA
                CreatedAt = DateTime.UtcNow,      // SỬA: Dùng UtcNow cho nhất quán
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(UserDTO userDto)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userDto.UserID); // SỬA: UserId -> UserID
            if (user == null) throw new Exception("User not found");

            // SỬA LỖI LOGIC: Cập nhật RoleID
            if (!string.IsNullOrEmpty(userDto.RoleName) && user.UserRole?.RoleName != userDto.RoleName)
            {
                var role = (await _unitOfWork.UserRoles.FindAsync(r => r.RoleName == userDto.RoleName)).FirstOrDefault();
                if (role == null)
                    throw new Exception($"Role '{userDto.RoleName}' not found.");
                user.RoleID = role.RoleID; // SỬA
            }

            user.FullName = userDto.FullName;
            user.Email = userDto.Email; // Cẩn thận: Có nên cho đổi email?
            user.DOB = userDto.DOB;
            user.AvatarURL = userDto.AvatarURL;   // SỬA
            user.UpdatedAt = DateTime.UtcNow; // SỬA: Dùng UtcNow

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            // Hàm này "miễn nhiễm" với các thay đổi, nó vẫn chạy tốt.
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null) return false;

            _unitOfWork.Users.Remove(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}