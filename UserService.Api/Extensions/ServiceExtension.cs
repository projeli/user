using UserService.Application.Services;

namespace UserService.Extensions;

public static class ServiceExtension
{
    public static void AddUserServiceServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, Application.Services.UserService>();
    }
}