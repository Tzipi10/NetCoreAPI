using Microsoft.AspNetCore.Mvc;
using MyApi.Models;
using MyApi.Interfaces;
namespace MyApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private IUserService UserService;
    public UserController(IUserService UserService){
        this.UserService = UserService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<User>> Get()
    {
        return UserService.Get();
    }

    [HttpGet("{id}")]
    public ActionResult<User> Get(int id)
    {
        var user = UserService.Get(id);
        if (user == null)
            return NotFound();
        return user;
    }

    [HttpPost]
    public ActionResult Post(User newUser)
    {
        var newId = UserService.Insert(newUser);
        if (newId == -1)
            return BadRequest();
        return CreatedAtAction(nameof(Post), new { Id= newId});
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, User newUser)
    {
        if(UserService.Update(id,newUser))
            return NoContent();

        return BadRequest();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        if (UserService.Delete(id))
            return Ok();
            
        return NotFound();
    }   
}
