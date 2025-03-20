using Microsoft.AspNetCore.Mvc;
using MyApi.Models;

namespace MyApi.Interfaces;

public interface IUserService
{
    List<User> Get();
    User Get(int id);

    int Insert(User newUser);


    bool Update(int id, User newUser);
    
    bool Delete(int id);
    
}
