using MassTransit;
using Projeli.UserService.Domain.Repositories;

namespace Projeli.UserService.Infrastructure.Repositories;

public class BusRepository(IBus bus) : IBusRepository
{
    public Task Publish(object message)
    {
        return bus.Publish(message);
    }
}