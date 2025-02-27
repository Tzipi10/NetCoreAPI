using Microsoft.AspNetCore.Mvc;
using MyApi.Models;

namespace MyApi.Interfaces;

public interface IGiftService
{
    List<Gift> Get();
    Gift Get(int id);

    int Insert(Gift newGift);


    bool Update(int id, Gift newGift);
    
    bool Delete(int id);
    
}
