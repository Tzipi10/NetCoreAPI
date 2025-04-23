using Microsoft.AspNetCore.Mvc;
using MyApi.Models;
using MyApi.Interfaces;

namespace MyApi.Controllers
{
    [Route("user")]
    public class UserController : GenericController<User>
    {
        public UserController(IUserService userService) : base(userService)
        {
        }
    }
}
