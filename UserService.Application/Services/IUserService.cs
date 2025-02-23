using UserService.Domain.Models;
using UserService.Domain.Results;

namespace UserService.Application.Services;

public interface IUserService
{
    Task<IResult<List<ProjeliUser>?>> GetByIds(List<string> ids);
    
    Task<IResult<ProjeliUser?>> GetById(string id);
}