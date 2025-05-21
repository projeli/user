using System.ComponentModel;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Projeli.Shared.Infrastructure.Converters;
using Projeli.UserService.Domain.Models;

namespace Projeli.UserService.Infrastructure.Database;

public class UserServiceDbContext(DbContextOptions<UserServiceDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<Ulid>()
            .HaveConversion<UlidToStringConverter>()
            .HaveConversion<UlidToGuidConverter>();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                var memberInfo = property.PropertyInfo ?? (MemberInfo?)property.FieldInfo;
                if (memberInfo == null) continue;
                var defaultValue =
                    Attribute.GetCustomAttribute(memberInfo, typeof(DefaultValueAttribute)) as DefaultValueAttribute;
                if (defaultValue == null) continue;
                property.SetDefaultValue(defaultValue.Value);
            }
        }

        builder.ApplyConfigurationsFromAssembly(typeof(UserServiceDbContext).Assembly);
    }
}