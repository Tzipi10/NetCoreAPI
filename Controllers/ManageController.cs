
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MyApi.Models;
using MyApi.Services;

namespace MyApi.Controllers;
[ApiController]
[Route("[controller]")]

public class ManageController : ControllerBase
{
    public ManageController() {}

    [HttpPost]
    [Route("[action]")]

    public ActionResult<String> Login([FromBody] User User)
        {
            var dt = DateTime.Now;

            if (User.Name != "MeRez"
            || User.Password != $"W{dt.Year}#{dt.Day}!")
            {
                return Unauthorized();
            }

            var claims = new List<Claim>
            {
                new Claim("type", "Admin"),
                new Claim("ClearanceLevel", "2"),
            };

            var token = TokenService.GetToken(claims);

            return new OkObjectResult(TokenService.WriteToken(token));
        }
}