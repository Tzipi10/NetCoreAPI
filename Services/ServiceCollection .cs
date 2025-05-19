using MyApi.Interfaces;
namespace MyApi.Services;
public static class ServiceCollection
{
    public static void AddCurrentUser(this IServiceCollection services)
    {
        services.AddSingleton<CurrentUserService>();
    }

    public static void AddGiftJson(this IServiceCollection services)
    {
        services.AddSingleton<IGiftService, GiftServiceJson>();
    }

    public static void AddUserJson(this IServiceCollection services)
    {
        services.AddSingleton<IUserService, UserServiceJson>();
    }
}