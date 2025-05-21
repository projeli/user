using MassTransit;
using Projeli.Shared.Infrastructure.Exceptions;
using Projeli.Shared.Infrastructure.Messaging.Events;

namespace Projeli.UserService.Api.Extensions;

public static class RabbitMqExtension
{
    public static void UseUserServiceRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, config) =>
            {
                config.Host(configuration["RabbitMq:Host"] ?? throw new MissingEnvironmentVariableException("RabbitMq:Host"), "/", h =>
                {
                    h.Username(configuration["RabbitMq:Username"] ?? throw new MissingEnvironmentVariableException("RabbitMq:Username"));
                    h.Password(configuration["RabbitMq:Password"] ?? throw new MissingEnvironmentVariableException("RabbitMq:Password"));
                });
                
                config.PublishFanOut<UserDeletedEvent>();
            });
        });
    }

    private static void PublishFanOut<T>(this IRabbitMqBusFactoryConfigurator configurator)
        where T : class
    {
        configurator.Publish<T>(y => y.ExchangeType = "fanout");
    }
}