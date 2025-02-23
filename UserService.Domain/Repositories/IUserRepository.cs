
using UserService.Domain.Models;

namespace UserService.Domain.Repositories;

public interface IUserRepository
{
    Task<List<ProjeliUser>?> GetByIds(List<string> ids);
    
    Task<ProjeliUser?> GetById(string id);
}