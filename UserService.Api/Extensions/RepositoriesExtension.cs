using UserService.Domain.Repositories;
using UserService.Infrastructure.Repositories;

namespace UserService.Extensions;

public static class RepositoriesExtension
{
    public static void AddUserServiceRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
    }
}