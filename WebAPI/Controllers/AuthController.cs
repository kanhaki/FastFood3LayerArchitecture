using BUS.Services;
using DTO.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth) { _auth = auth; }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupRequest req)
        {
            try
            {
                var id = await _auth.SignUpAsync(req);
                return Created("", new { userId = id });
            }
            catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
            catch (InvalidOperationException ex) { return Conflict(new { error = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { error = "Server error", detail = ex.Message }); }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            try
            {
                var res = await _auth.LoginAsync(req);
                var cookieOptions = new CookieOptions { HttpOnly = true, Secure = true, Expires = res.ExpiresAt, SameSite = SameSiteMode.Strict };
                Response.Cookies.Append("access_token", res.Token, cookieOptions);
                return Ok(new { userId = res.UserId });
            }
            catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
            catch (UnauthorizedAccessException ex) { return Unauthorized(new { error = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { error = "Server error", detail = ex.Message }); }
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var token = Request.Cookies["access_token"];
            await _auth.LogoutAsync(token ?? string.Empty);
            Response.Cookies.Delete("access_token");
            return Ok(new { message = "Logged out" });
        }

        [HttpPost("session")]
        public async Task<IActionResult> Session()
        {
            var token = Request.Cookies["access_token"];
            var info = await _auth.ValidateTokenAsync(token ?? string.Empty);
            if (info == null) return Unauthorized(new { error = "No active session" });
            return Ok(info);
        }
    }
}
