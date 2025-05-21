using Projeli.Shared.Domain.Results;
using Projeli.UserService.Application.Dtos;

namespace Projeli.UserService.Application.Services;

public interface IUserService
{
    Task<IResult<List<UserDto>>> GetByIds(List<string> ids);
    Task<IResult<List<UserDto>>> SearchByUsername(string username);
    Task<IResult<UserDto?>> GetById(string id);
    Task<IResult<UserDto?>> Create(UserDto user);
    Task<IResult<UserDto?>> Update(string id, UserDto user);
    Task<IResult<UserDto?>> Delete(string id);
}