using DAT.UnitOfWork;
using Common;
using DTO.DTO;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
                || string.IsNullOrWhiteSpace(dto.Role))
                throw new ArgumentException("Missing required fields");

            var allowedRoles = new[] { "customer", "admin", "staff" };
            if (!allowedRoles.Contains(dto.Role.ToLowerInvariant()))
                throw new ArgumentException("Invalid role");

            var existing = await _uow.Users.GetByEmailAsync(dto.Email);
            if (existing != null)
                throw new InvalidOperationException("Email already exists");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new DAT.Entity.User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = passwordHash,
                Role = dto.Role.ToLowerInvariant(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _uow.Users.AddAsync(user);
            await _uow.SaveChangesAsync();

            return user.UserId;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                throw new ArgumentException("Missing email or password");

            var user = (await _uow.Users.FindAsync(u => u.Email == dto.Email)).FirstOrDefault();
            if (user == null) throw new UnauthorizedAccessException("Invalid credentials");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials");

            if (!user.IsActive) throw new UnauthorizedAccessException("User inactive");

            var jwtSettings = _config.GetSection("Jwt");
            var token = JwtHelper.GenerateToken(user, jwtSettings);
            var expires = JwtHelper.GetExpiryFromSettings(jwtSettings);

            return new AuthResponse
            {
                UserId = user.UserId,
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
            if (string.IsNullOrWhiteSpace(token)) return null;
            //if (_blacklist != null && await _blacklist.IsBlacklistedAsync(token)) return null;

            var principal = JwtHelper.ValidateToken(token, _config.GetSection("Jwt"));
            if (principal == null) return null;

            var userId = long.Parse(principal.FindFirst("sub")!.Value);
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
