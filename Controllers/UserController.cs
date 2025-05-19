using Microsoft.AspNetCore.Mvc;
using MyApi.Models;
using MyApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using MyApi.Services;
namespace MyApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private IUserService UserService;

    //to delete user items in delete user
    private IGiftService GiftService;
    public UserController(IUserService UserService, IGiftService GiftService){
        this.UserService = UserService;
        this.GiftService = GiftService;

    }


    [HttpGet]
    [Authorize(Policy = "Admin")]

    public ActionResult<IEnumerable<User>> Get()
    {
        return UserService.Get();
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "User")]
    public ActionResult<User> Get(int id)
    {
        
        // if(id!=currentUser.UserId && User.FindFirst("type")?.Value != "Admin")
        //     return Unauthorized();
        var user = UserService.Get(id);
        if (user == null)
            return NotFound();
        return user;
    }

    [HttpPost]
    [Authorize(Policy = "Admin")]

    public ActionResult Post(User newUser)
    {
        var userId = UserService.Insert(newUser);
        if (userId == -1)
            return BadRequest();
        return CreatedAtAction(nameof(Post), new { Id= userId});
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "User")]

    public ActionResult Put(int id, User newUser)
    {
        if(UserService.Update(id,newUser))
            return NoContent();

        return BadRequest();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Admin")]

    public ActionResult Delete(int id)
    {
        GiftService.DeleteUserItems(id);

        if(UserService.Delete(id))
            return Ok();
            
        return NotFound();
    } 

}