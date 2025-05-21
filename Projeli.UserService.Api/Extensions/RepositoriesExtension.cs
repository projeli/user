using Projeli.UserService.Domain.Repositories;
using Projeli.UserService.Infrastructure.Repositories;

namespace Projeli.UserService.Api.Extensions;

public static class RepositoriesExtension
{
    public static void AddUserServiceRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
    }
}