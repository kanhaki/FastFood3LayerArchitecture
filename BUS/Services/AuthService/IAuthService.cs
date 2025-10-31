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
        Task<int> SignUpAsync(SignupRequest dto);
        Task<AuthResponse> LoginAsync(LoginRequest dto);
        Task LogoutAsync(int token);
        Task<AuthResponse> RefreshTokenAsync(string refreshToken);

    }
}
