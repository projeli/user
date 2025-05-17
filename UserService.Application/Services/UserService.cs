using UserService.Domain.Models;
using UserService.Domain.Repositories;
using UserService.Domain.Results;

namespace UserService.Application.Services;

public class UserService(IUserRepository repository) : IUserService
{
    public async Task<IResult<List<ProjeliUser>>> GetByIds(List<string> ids)
    {
        var users = await repository.GetByIds(ids);
        return new Result<List<ProjeliUser>>(users);
    }

    public async Task<IResult<List<ProjeliUser>>> SearchByUsername(string username)
    {
        var users = await repository.SearchByUsername(username);
        return new Result<List<ProjeliUser>>(users);
    }

    public async Task<IResult<ProjeliUser?>> GetById(string id)
    {
        var user = await repository.GetById(id);
        return new Result<ProjeliUser?>(user);
    }
}