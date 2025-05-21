using Microsoft.EntityFrameworkCore;
using Projeli.Shared.Infrastructure.Exceptions;
using Projeli.UserService.Infrastructure.Database;

namespace Projeli.UserService.Api.Extensions;

public static class DatabaseExtension
{
    public static void AddUserServiceDatabase(this IServiceCollection services, IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        var connectionString = configuration["Database:ConnectionString"];
        
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new MissingEnvironmentVariableException("Database:ConnectionString");
        }

        services.AddDbContext<UserServiceDbContext>(options =>
        {
            options.UseNpgsql(connectionString, builder => { builder.MigrationsAssembly("Projeli.UserService.Api"); });
            
            if (environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
            }
        });
    }

    public static void UseUserServiceDatabase(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var database = scope.ServiceProvider.GetRequiredService<UserServiceDbContext>();
        if (database.Database.GetPendingMigrations().Any())
        {
            database.Database.Migrate();
        }
    }
}