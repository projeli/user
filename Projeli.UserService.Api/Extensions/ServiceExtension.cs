using Projeli.UserService.Application.Services;

namespace Projeli.UserService.Api.Extensions;

public static class ServiceExtension
{
    public static void AddUserServiceServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, Application.Services.UserService>();
    }
}