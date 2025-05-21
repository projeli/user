namespace Projeli.UserService.Domain.Repositories;

public interface IBusRepository
{
    Task Publish(object message);
}