using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Services
{
    public interface IAuthService
    {
        Task<long> SignUpAsync(SignupRequest dto);
        Task<AuthResponse> LoginAsync(LoginRequest dto);
        Task LogoutAsync(string token);
        Task<SessionInfo?> ValidateTokenAsync(string token);
    }
}
