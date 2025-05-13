using Microsoft.AspNetCore.Mvc;
using MyApi.Models;

namespace MyApi.Interfaces
{
    public interface IUserService : IService<User>
    {
        int ExistUserId(string name,string password);
        
    }
}
