using MyApi.Models;

namespace MyApi.Services;

public  class CurrentUserService {
     public int UserId { get; set; }
     public bool IsAdmin { get; set; }

        public CurrentUserService() { 
            // UserId = userId;
        }



}

