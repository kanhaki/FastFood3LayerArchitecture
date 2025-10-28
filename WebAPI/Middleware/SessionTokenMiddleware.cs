using Common;

namespace WebAPI.Middleware
{
    public class SessionTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;
        public SessionTokenMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _config = config;
        }
        public async Task Invoke(HttpContext context)
        {
            string? token = context.Request.Cookies["access_token"];

            if (string.IsNullOrWhiteSpace(token))
            {
                var header = context.Request.Headers["Authorization"].FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(header) && header.StartsWith("Bearer "))
                    token = header.Substring("Bearer ".Length);
            }

            if (!string.IsNullOrWhiteSpace(token))
            {
                var principal = JwtHelper.ValidateToken(token, _config.GetSection("Jwt"));
                if (principal != null)
                    context.User = principal;
            }
            await _next(context);
        }
    }
}
