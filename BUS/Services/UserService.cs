using DAT.UnitOfWork;
using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var users = await _unitOfWork.Users.GetAllAsync();
            return users.Select(u => new UserDTO
            {
                UserId = u.UserId,
                FullName = u.FullName,
                Email = u.Email,
                DOB = u.DOB,
                Role = u.Role,
                AvatarUrl = u.AvatarUrl,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt,
            });
        }

        public async Task<UserDTO?> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null) return null;

            return new UserDTO
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                DOB = user.DOB,
                Role = user.Role,
                AvatarUrl = user.AvatarUrl,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
            };
        }

        public async Task AddUserAsync(UserDTO userDto)
        {
            var user = new DAT.Entity.User
            {
                FullName = userDto.FullName,
                Email = userDto.Email,
                PasswordHash = "123456", // default
                DOB = userDto.DOB,
                Role = string.IsNullOrEmpty(userDto.Role) ? "customer" : userDto.Role, // fix
                AvatarUrl = userDto.AvatarUrl,
                IsActive = userDto.IsActive,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now

            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
