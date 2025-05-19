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
public class GiftServiceJson : IGiftService
{
    List<Gift> Gifts { get;}
    CurrentUserService currentUser;
    private static string fileName = "gift.json";
    private string filePath;
    public GiftServiceJson(IHostEnvironment env,CurrentUserService currentUser)
    {
        filePath = Path.Combine(env.ContentRootPath, "Data", fileName);
        using (var jsonFile = File.OpenText(filePath))
        {
            Gifts = JsonSerializer.Deserialize<List<Gift>>(jsonFile.ReadToEnd(),
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        this.currentUser = currentUser;
    }

    private void savaToFile()
    {
        File.WriteAllText(filePath, JsonSerializer.Serialize(Gifts));
    }
    public List<Gift> Get()
    {
        Console.WriteLine(currentUser.UserId);
        if(currentUser.IsAdmin)
            return Gifts;
        return Gifts.FindAll(g => g.UserId == currentUser.UserId);
    }

    public Gift Get(int id)
    {
        return Gifts.FirstOrDefault(g => g.Id == id && g.UserId == currentUser.UserId);
    }
    public int Insert(Gift newGift)
    {
        Console.WriteLine("in insert, "+newGift.Price, ", " + newGift.Name);
        if (newGift == null
            || string.IsNullOrEmpty(newGift.Name)
            || newGift.Price <= 0)
            return -1;

        newGift.UserId = currentUser.UserId;
        newGift.Id = Gifts.Max(g => g.Id) + 1;
        Gifts.Add(newGift);
        savaToFile();
        return newGift.Id;
    }

    public bool Update(int id, Gift newGift)
    {
        if (newGift == null
            || string.IsNullOrEmpty(newGift.Name)
            || newGift.Price <= 0
            || newGift.Id != id)
            return false;

        var gift = Gifts.FirstOrDefault(g => g.Id == id);
        if(gift.UserId !=currentUser.UserId && !currentUser.IsAdmin)
            return false;
        gift.Name = newGift.Name;
        gift.Price = newGift.Price;
        gift.Summary = newGift.Summary;
        savaToFile();
        return true;
    }
    public bool Delete(int id)
    {
        var gift = Gifts.FirstOrDefault(g => g.Id == id);
        if (gift == null || (gift.UserId != currentUser.UserId && !currentUser.IsAdmin))
            return false;

        Gifts.RemoveAt(Gifts.IndexOf(gift));
        savaToFile();
        return true;
    }

    public void DeleteUserItems(int userId)
    {

        Gifts.RemoveAll(g => g.UserId == userId);
        
        savaToFile();
         
    }
}

