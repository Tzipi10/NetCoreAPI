using MyApi.Models;

namespace MyApi.Services;

public  class CurrentUserService {
     public int UserId { get; private set; }

        public CurrentUserService() { }

        public void UpdateUserId(int userId)
        {
            UserId = userId;
            Console.WriteLine(userId);
        }



}