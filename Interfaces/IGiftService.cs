using Microsoft.AspNetCore.Mvc;
using MyApi.Models;

namespace MyApi.Interfaces
{
    public interface IGiftService : IService<Gift>
    {
        void DeleteUserItems(int userId);
    }
}
