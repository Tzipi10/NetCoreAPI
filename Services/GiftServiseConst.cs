using MyApi.Models;
using MyApi.Interfaces;

namespace MyApi.Services;
public class GiftServiceConst /*: IGiftService*/
 {
    private List<Gift> list;

    public GiftServiceConst()
    {
        list = new List<Gift>
        {
            new Gift {Id = 1, Name = "jewelry",Price = 500,Summary = "jewelry set"},
            new Gift {Id = 2, Name = "ornaments",Price = 400,Summary = "Decoration shelf"}
        };
    }
    public List<Gift> Get()
    {
        return list;
    }

    public Gift Get(int id)
    {
        return list.FirstOrDefault(g => g.Id == id);
    }
    public int Insert(Gift newGift)
    {
        if(newGift == null 
            || string.IsNullOrEmpty(newGift.Name)
            || newGift.Price <= 0
            || newGift.UserId <0)
            return -1;
        
        newGift.Id = list.Max(g => g.Id) +1;
        list.Add(newGift);
        return newGift.Id;
    }

    public bool Update(int id, Gift newGift)
    {
        if(newGift == null 
            || string.IsNullOrEmpty(newGift.Name)
            || newGift.Price <= 0
            || newGift.UserId <0
            || newGift.Id != id)
            return false;
        
        var gift = list.FirstOrDefault(g => g.Id == id);
        gift.Name = newGift.Name;
        gift.Price = newGift.Price;
        gift.UserId = newGift.UserId;
        gift.Summary = newGift.Summary;
        return true;
    }
    public bool Delete(int id)
    {
        var gift = list.FirstOrDefault(g => g.Id == id);
        if(gift == null)
            return false;
        
        list.RemoveAt(list.IndexOf(gift));

        return true;
    }
}

// public static class GiftUtilities
// {
//     public static void AddGiftConst(this IServiceCollection services)
//     {
//         services.AddSingleton<IGiftService,GiftServiceConst>();
//     }
// }