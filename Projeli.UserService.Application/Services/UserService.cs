using AutoMapper;
using Projeli.Shared.Domain.Results;
using Projeli.UserService.Application.Dtos;
using Projeli.UserService.Domain.Models;
using Projeli.UserService.Domain.Repositories;

namespace Projeli.UserService.Application.Services;

public class UserService(IUserRepository repository, IMapper mapper) : IUserService
{
    public async Task<IResult<List<UserDto>>> GetByIds(List<string> ids)
    {
        var users = await repository.GetByIds(ids);
        return new Result<List<UserDto>>(mapper.Map<List<UserDto>>(users));
    }

    public async Task<IResult<List<UserDto>>> SearchByUsername(string username)
    {
        var users = await repository.SearchByUsername(username);
        return new Result<List<UserDto>>(mapper.Map<List<UserDto>>(users));
    }

    public async Task<IResult<UserDto?>> GetById(string id)
    {
        var user = await repository.GetById(id);
        return new Result<UserDto?>(mapper.Map<UserDto?>(user));
    }

    public async Task<IResult<UserDto?>> Create(UserDto user)
    {
        var existingUser = await repository.GetById(user.UserId);
        if (existingUser is not null)
        {
            return Result<UserDto?>.Fail("User already exists.");
        }

        user.Id = Ulid.NewUlid();

        var newUser = mapper.Map<User>(user);
        await repository.Create(newUser);

        return new Result<UserDto?>(mapper.Map<UserDto?>(newUser));
    }

    public async Task<IResult<UserDto?>> Update(string id, UserDto user)
    {
        var existingUser = await repository.GetById(id);
        if (existingUser is null)
        {
            return Result<UserDto?>.NotFound();
        }

        var updatedUser = mapper.Map(user, existingUser);
        await repository.Update(id, updatedUser);

        return new Result<UserDto?>(mapper.Map<UserDto?>(updatedUser));
    }

    public async Task<IResult<UserDto?>> Delete(string id)
    {
        var existingUser = await repository.GetById(id);
        if (existingUser is null)
        {
            return Result<UserDto?>.NotFound();
        }

        await repository.Delete(id);

        return new Result<UserDto?>(mapper.Map<UserDto?>(existingUser));
    }
}