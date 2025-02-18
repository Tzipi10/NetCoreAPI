using Microsoft.AspNetCore.Mvc;
using MyApi.Models;

namespace MyApi.Services;

[ApiController]
[Route("[controller]")]
public static class GiftService
{
    private static List<Gift> list;

    static GiftService()
    {
        list = new List<Gift>
        {
            new Gift {Id = 1, Name = "jewelry",Price = 500,Summary = "jewelry set"},
            new Gift {Id = 2, Name = "ornaments",Price = 400,Summary = "Decoration shelf"}
        };
    }
    public static List<Gift> Get()
    {
        return list;
    }

    public static Gift Get(int id)
    {
        return list.FirstOrDefault(g => g.Id == id);
    }
    public static int Insert(Gift newGift)
    {
        if(newGift == null 
            || string.IsNullOrEmpty(newGift.Name)
            || newGift.Price <= 0)
            return -1;
        
        newGift.Id = list.Max(g => g.Id) +1;
        list.Add(newGift);
        return newGift.Id;
    }

    public static bool Update(int id, Gift newGift)
    {
        if(newGift == null 
            || string.IsNullOrEmpty(newGift.Name)
            || newGift.Price <= 0
            || newGift.Id != id)
            return false;
        
        var gift = list.FirstOrDefault(g => g.Id == id);
        gift.Name = newGift.Name;
        gift.Price = newGift.Price;
        gift.Summary = gift.Summary;
        return true;
    }
    public static bool Delete(int id)
    {
        var gift = list.FirstOrDefault(g => g.Id == id);
        if(gift == null)
            return false;
        
        list.RemoveAt(list.IndexOf(gift));

        return true;
    }
}
