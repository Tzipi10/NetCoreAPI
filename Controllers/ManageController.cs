
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MyApi.Models;
using MyApi.Services;
using Microsoft.AspNetCore.Authorization;
using MyApi.Interfaces;

namespace MyApi.Controllers;
[ApiController]
[Route("[controller]")]
public class ManageController : ControllerBase
{
    private IUserService UserService;

    public ManageController(IUserService UserService)
    {
        this.UserService = UserService;
    }

    [HttpPost]
    [Route("[action]")]

    public ActionResult<String> Login([FromBody] userRequest user)
    {
        int userId = UserService.ExistUserId(user.Name, user.Password);
        if (userId == -1)
            return Unauthorized();
        var claims = new List<Claim> { };
        if (user.Name == "MeReTz" && user.Password == "CLicK")
        {
            claims.Add(new Claim("type", "Admin"));
        }
        else
        {
            claims.Add(new Claim("type", "User"));
        }

        claims.Add(new Claim("userId", userId.ToString()));
        var token = TokenService.GetToken(claims);
        //Console.WriteLine(TokenService.WriteToken(token)); // הדפסת הטוקן

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
            //
        };

        var token = TokenService.GetToken(claims);

        return new OkObjectResult(TokenService.WriteToken(token));

    }

}

public class userRequest{
    public string Name { get; set; }
    public string Password { get; set; }
}