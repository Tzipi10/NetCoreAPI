using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using MyApi.Services;
namespace MyApi.Middlewares;

public class TokenMiddleware
{
    private readonly RequestDelegate _next;

    public TokenMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // בדוק אם יש טוקן ב-Authorization header
        if (context.Request.Headers.TryGetValue("Authorization", out var tokenHeader))
        {
            var token = tokenHeader.ToString().Replace("Bearer ", "");
           // Console.WriteLine("token in middle: " + token);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId");
            var type = jwtToken.Claims.FirstOrDefault(c => c.Type == "type").Value;
           // Console.WriteLine("userId in middle: " + userIdClaim + ", type = " + type);

            if (userIdClaim != null)
            {
                var currentUserService = context.RequestServices.GetService<CurrentUserService>();
                currentUserService.UserId = int.Parse(userIdClaim.Value);
                currentUserService.IsAdmin = type == "Admin";

            }
        }

        await _next(context);
    }
}
