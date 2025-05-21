
using Projeli.UserService.Domain.Models;

namespace Projeli.UserService.Domain.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetByIds(List<string> ids);
    Task<List<User>> SearchByUsername(string username);
    Task<User?> GetById(string id);
    Task<User?> Create(User user);
    Task<User?> Update(string userId, User user);
    Task<bool> Delete(string id);
}