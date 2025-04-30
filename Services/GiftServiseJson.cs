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
    // private List<Gift> list;
    List<Gift> Gifts { get; }
    CurrentUserService currentUser;
    private static string fileName = "gift.json";
    private string filePath;
    public GiftServiceJson(IHostEnvironment env)
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
    }

    private void savaToFile()
    {
        File.WriteAllText(filePath, JsonSerializer.Serialize(Gifts));
    }
    public List<Gift> Get()
    {
        return Gifts.FindAll(g => g.UserId == currentUser.UserId);
    }

    public Gift Get(int id)
    {
        return Gifts.FirstOrDefault(g => g.Id == id);
    }
    public int Insert(Gift newGift)
    {
        if (newGift == null
            || string.IsNullOrEmpty(newGift.Name)
            || newGift.Price <= 0
            || newGift.UserId < 0)
            return -1;

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
            || newGift.Id != id
            || newGift.UserId < 0)
            return false;

        var gift = Gifts.FirstOrDefault(g => g.Id == id);
        gift.Name = newGift.Name;
        gift.Price = newGift.Price;
        gift.UserId = newGift.UserId;
        gift.Summary = newGift.Summary;
        savaToFile();
        return true;
    }
    public bool Delete(int id)
    {
        var gift = Gifts.FirstOrDefault(g => g.Id == id);
        if (gift == null)
            return false;

        Gifts.RemoveAt(Gifts.IndexOf(gift));
        savaToFile();
        return true;
    }
}

public static class GiftUtilities
{
    public static void AddGiftJson(this IServiceCollection services)
    {
        services.AddSingleton<IGiftService, GiftServiceJson>();
    }
}