using MyApi.Models;
using MyApi.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Text.Json;
//using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MyApi.Services;
public class UserServiceJson: IUserService
{
    // private List<User> list;
    List<User> Users{get;}

    private static string fileName = "user.json";
    private string filePath;
    public UserServiceJson(IHostEnvironment env)
    {
        filePath = Path.Combine(env.ContentRootPath,"Data",fileName);
        using(var jsonFile = File.OpenText(filePath))
        {
            Users = JsonSerializer.Deserialize<List<User>>(jsonFile.ReadToEnd(),
            new JsonSerializerOptions
            {
                 PropertyNameCaseInsensitive = true
            });
        }
    }

    private void savaToFile()
    {
        File.WriteAllText(filePath,JsonSerializer.Serialize(Users));
    }
    public List<User> Get() => Users;

    public User Get(int id)
    {
        return Users.FirstOrDefault(u => u.Id == id);
    }
    public int Insert(User newUser)
    {
        if(newUser == null 
            || string.IsNullOrEmpty(newUser.Name)
            || string.IsNullOrEmpty(newUser.Password)
            || newUser.PermissionLevel <=0 || newUser.PermissionLevel >2)
            return -1;
        
        newUser.Id = Users.Max(u => u.Id) +1;
        Users.Add(newUser);
        savaToFile();
        return newUser.Id;
    }

    public bool Update(int id, User newUser)
    {
        if(newUser == null 
            || string.IsNullOrEmpty(newUser.Name)
            || string.IsNullOrEmpty(newUser.Password)
            || newUser.PermissionLevel <=0 || newUser.PermissionLevel >2
            || newUser.Id != id)
            return false;
        
        var user = Users.FirstOrDefault(u => u.Id == id);
        user.Name = newUser.Name;
        user.Password = newUser.Password;
        user.PermissionLevel = newUser.PermissionLevel;
        savaToFile();
        return true;
    }
    public bool Delete(int id)
    {
        var user = Users.FirstOrDefault(u => u.Id == id);
        if(user == null)
            return false;
        
        Users.RemoveAt(Users.IndexOf(user));
        savaToFile();
        return true;
    }
}

public static class UserUtilities
{
    public static void AddUserJson(this IServiceCollection services)
    {
        services.AddSingleton<IUserService,UserServiceJson>();
    }
}