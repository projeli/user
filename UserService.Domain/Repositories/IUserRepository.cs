
using UserService.Domain.Models;

namespace UserService.Domain.Repositories;

public interface IUserRepository
{
    Task<List<ProjeliUser>> GetByIds(List<string> ids);
    Task<List<ProjeliUser>> SearchByUsername(string username);
    Task<ProjeliUser?> GetById(string id);
}