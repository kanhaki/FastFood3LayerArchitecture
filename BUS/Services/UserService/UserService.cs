using DAT.UnitOfWork;
using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAT.Entity;
using Microsoft.EntityFrameworkCore;

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
            return users.Select(MapToDTO);
        }

        public async Task<UserDTO?> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdWithRoleAsync(id);
            if (user == null) return null;
            return MapToDTO(user);
        }

        public async Task<UserDTO> AddUserAsync(UserDTO userDto) // SỬA: Trả về UserDTO
        {
            var roleName = string.IsNullOrEmpty(userDto.RoleName) ? "customer" : userDto.RoleName;
            var role = (await _unitOfWork.UserRoles.FindAsync(r => r.RoleName == roleName)).FirstOrDefault();

            // SỬA: "Ném" lỗi 400 (Bad Request) nếu Role tào lao
            if (role == null)
                throw new ArgumentException($"Role '{roleName}' not found.");

            if (string.IsNullOrWhiteSpace(userDto.Password))
                throw new ArgumentException("Password is required to create a user.");

            // Kiểm tra Email trùng (bạn đã làm ở AuthService, nên làm ở đây luôn)
            if (await _unitOfWork.Users.GetByEmailAsync(userDto.Email) != null)
                throw new InvalidOperationException("Email already exists."); // Lỗi 409 Conflict

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            var user = new DAT.Entity.User
            {
                FullName = userDto.FullName,
                Email = userDto.Email,
                PasswordHash = passwordHash,
                DOB = userDto.DOB,
                RoleID = role.RoleID,
                AvatarURL = userDto.AvatarURL,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return MapToDTO(user);
        }

        public async Task<bool> UpdateUserAsync(int id, UserDTO userDto) // SỬA: Thêm 'id' và trả về 'bool'
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);

            // SỬA: "Ném" lỗi 404 (Not Found)
            if (user == null)
                return false;

            // Cập nhật RoleID (nếu có)
            if (!string.IsNullOrEmpty(userDto.RoleName) && user.UserRole?.RoleName != userDto.RoleName)
            {
                var role = (await _unitOfWork.UserRoles.FindAsync(r => r.RoleName == userDto.RoleName)).FirstOrDefault();

                // SỬA: "Ném" lỗi 400 (Bad Request) nếu Role tào lao
                if (role == null)
                    throw new ArgumentException($"Role '{userDto.RoleName}' not found.");
                user.RoleID = role.RoleID;
            }

            user.FullName = userDto.FullName;
            user.Email = userDto.Email; // CẨN THẬN: Đổi email có thể cần xác thực lại
            user.DOB = userDto.DOB;
            user.AvatarURL = userDto.AvatarURL;
            user.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var entity = await _unitOfWork.Users.GetByIdAsync(id);
            if (entity == null) return false;

            _unitOfWork.Users.Remove(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        private UserDTO MapToDTO(DAT.Entity.User u)
        {
            return new UserDTO
            {
                UserID = u.UserID,
                FullName = u.FullName,
                Email = u.Email,
                DOB = u.DOB,
                RoleID = u.RoleID,
                RoleName = u.UserRole?.RoleName,
                AvatarURL = u.AvatarURL,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt,
            };
        }
    }
}