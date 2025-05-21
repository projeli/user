using Projeli.UserService.Api.Middlewares;

namespace Projeli.UserService.Api.Extensions;

public static class MiddlewareExtension
{
    public static void UseUserServiceMiddleware(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<DatabaseExceptionMiddleware>();
        builder.UseMiddleware<HttpExceptionMiddleware>();
    }
}