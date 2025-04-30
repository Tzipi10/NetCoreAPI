
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MyApi.Models;
using MyApi.Services;
using Microsoft.AspNetCore.Authorization;

namespace MyApi.Controllers;
[ApiController]
[Route("[controller]")]
public class ManageController : ControllerBase
{
    public ManageController() { }

    [HttpPost]
    [Route("[action]")]

    public ActionResult<String> Login([FromBody] User User)
    {
        if (User.Name != "MeReTz"
        || User.Password != "CLicK")
        {
            return Unauthorized();
        }

        var claims = new List<Claim>
            {
                new Claim("type", "Admin")
            };


        var token = TokenService.GetToken(claims);
        Console.WriteLine(TokenService.WriteToken(token)); // הדפסת הטוקן

        return new OkObjectResult(TokenService.WriteToken(token));
    }

    [HttpPost]
    [Route("[action]")]
    [Authorize(Policy = "Admin")]
    public IActionResult GenerateBadge([FromBody] User user)
    {
        var claims = new List<Claim>
        {
            new Claim("type", "User"),
        };

        var token = TokenService.GetToken(claims);

        return new OkObjectResult(TokenService.WriteToken(token));
    
    }

}