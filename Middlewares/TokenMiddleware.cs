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
            Console.WriteLine("token in middle: " + token);

            // פענח את הטוקן
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // שלוף את המידע מהטוקן (כמו UserId)
           var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId");
            Console.WriteLine("userId in middle: " + userIdClaim);

            if (userIdClaim != null)
            {
                var currentUserService = context.RequestServices.GetService<CurrentUserService>();
                currentUserService.UserId = int.Parse(userIdClaim.Value);
                Console.WriteLine("currentuser in middle: " + currentUserService);
                Console.WriteLine("currentuserid in middle: " + currentUserService.UserId);

            }
        }

        await _next(context);
    }
}
