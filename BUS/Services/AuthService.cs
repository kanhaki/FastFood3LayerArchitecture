using DAT.UnitOfWork;
using Common;
using DTO.DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DAT.Entity; // Đảm bảo bạn đã using namespace Entity

namespace BUS.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _config;
        //private readonly ITokenBlacklistRepository? _blacklist;

        public AuthService(IUnitOfWork uow, IConfiguration config)
        {
            _uow = uow;
            _config = config;
            // = blacklist;
        }

        public async Task<long> SignUpAsync(SignupRequest dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FullName)
                || string.IsNullOrWhiteSpace(dto.Email)
                || string.IsNullOrWhiteSpace(dto.Password)
                || string.IsNullOrWhiteSpace(dto.Role)) // Vẫn cần kiểm tra DTO
                throw new ArgumentException("Missing required fields");

            // THAY ĐỔI: Không dùng mảng 'allowedRoles' cứng nữa.
            // Chúng ta sẽ truy vấn thẳng vào bảng UserRole để tìm RoleID.
            var roleName = dto.Role.ToLowerInvariant();

            // Giả sử _uow.UserRoles là IGenericRepository<UserRole>
            var roleEntity = (await _uow.UserRoles.FindAsync(r => r.RoleName == roleName)).FirstOrDefault();

            if (roleEntity == null)
                throw new ArgumentException($"Invalid role specified: {dto.Role}");

            // Kiểm tra email tồn tại (như cũ)
            var existing = await _uow.Users.GetByEmailAsync(dto.Email);
            if (existing != null)
                throw new InvalidOperationException("Email already exists");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            // THAY ĐỔI: Tạo User mới
            var user = new DAT.Entity.User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = passwordHash,
                RoleID = roleEntity.RoleID, // <--- THAY ĐỔI TỪ Role (string) SANG RoleID (int)
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                // IsActive = true <--- ĐÃ XÓA
            };

            await _uow.Users.AddAsync(user);
            await _uow.SaveChangesAsync();

            return user.UserID;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                throw new ArgumentException("Missing email or password");

            // THAY ĐỔI: Dùng GetByEmailAsync cho nhất quán (như SignUp)
            // var user = (await _uow.Users.FindAsync(u => u.Email == dto.Email)).FirstOrDefault();
            var user = await _uow.Users.GetByEmailAsync(dto.Email);

            if (user == null) throw new UnauthorizedAccessException("Invalid credentials");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials");

            // if (!user.IsActive) throw new UnauthorizedAccessException("User inactive"); // <--- ĐÃ XÓA

            // THAY ĐỔI QUAN TRỌNG:
            // Để tạo JWT (token) với RoleName (ví dụ: "admin"),
            // chúng ta phải đảm bảo 'user.UserRole' đã được tải.
            if (user.UserRole == null)
            {
                // Lỗi này là dành cho DEV (là bạn):
                // Bạn PHẢI cập nhật 'GetByEmailAsync' trong 'UserRepository'
                // để nó dùng .Include(u => u.UserRole)
                throw new InvalidOperationException($"User role not loaded for user {user.Email}. " +
                    "Please update 'GetByEmailAsync' in your 'UserRepository' to use '.Include(u => u.UserRole)'.");
            }
            // Logic của JwtHelper.GenerateToken giờ sẽ dùng user.UserRole.RoleName
            // thay vì user.Role (không còn tồn tại)

            var jwtSettings = _config.GetSection("Jwt");
            var token = JwtHelper.GenerateToken(user, jwtSettings); // Giả sử JwtHelper đã được cập nhật để đọc user.UserRole.RoleName
            var expires = JwtHelper.GetExpiryFromSettings(jwtSettings);

            return new AuthResponse
            {
                UserId = user.UserID, // Sửa lỗi chính tả (UserId -> UserID)
                Token = token,
                ExpiresAt = expires
            };
        }

        public async Task LogoutAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return;
            /*if (_blacklist != null)
            {
                var exp = JwtHelper.GetExpiryFromToken(token);
                await _blacklist.BlacklistTokenAsync(token, exp);
            }*/
        }

        public async Task<SessionInfo?> ValidateTokenAsync(string token)
        {
            // Không cần thay đổi gì ở đây.
            // Nếu LoginAsync tạo token đúng (với RoleName),
            // thì hàm ValidateToken này sẽ đọc token đúng.

            if (string.IsNullOrWhiteSpace(token)) return null;
            //if (_blacklist != null && await _blacklist.IsBlacklistedAsync(token)) return null;

            var principal = JwtHelper.ValidateToken(token, _config.GetSection("Jwt"));
            if (principal == null) return null;

            // THAY ĐỔI: Sửa lỗi chính tả từ long.Parse(principal.FindFirst("sub")!.Value) thành int.Parse
            // vì UserID của bạn là INT, không phải LONG
            var userId = int.Parse(principal.FindFirst("sub")!.Value);
            var email = principal.FindFirst("email")!.Value;
            var role = principal.FindFirst(ClaimTypes.Role)!.Value;
            var exp = JwtHelper.GetExpiryFromToken(token);

            return new SessionInfo
            {
                UserId = userId,
                Email = email,
                Role = role,
                ExpiresAt = exp
            };
        }
    }
}