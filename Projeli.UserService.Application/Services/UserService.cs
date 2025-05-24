using AutoMapper;
using Projeli.Shared.Application.Messages.Users;
using Projeli.Shared.Domain.Results;
using Projeli.UserService.Application.Dtos;
using Projeli.UserService.Domain.Models;
using Projeli.UserService.Domain.Repositories;

namespace Projeli.UserService.Application.Services;

public class UserService(IUserRepository repository, IBusRepository busRepository, IMapper mapper) : IUserService
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
        newUser = await repository.Create(newUser);

        return newUser is not null
            ? new Result<UserDto?>(mapper.Map<UserDto?>(newUser))
            : Result<UserDto?>.Fail("Failed to create user.");
    }

    public async Task<IResult<UserDto?>> Update(string id, UserDto user)
    {
        var existingUser = await repository.GetById(id);
        if (existingUser is null)
        {
            user.UserId = id;
            return await Create(user);
        }

        existingUser.UserName = user.UserName;
        existingUser.Email = user.Email ?? existingUser.Email;
        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;
        existingUser.ImageUrl = user.ImageUrl;

        var updatedUser = await repository.Update(id, existingUser);
        
        return updatedUser is not null
            ? new Result<UserDto?>(mapper.Map<UserDto?>(updatedUser))
            : Result<UserDto?>.Fail("Failed to update user.");
    }

    public async Task<IResult<UserDto?>> Delete(string id)
    {
        var existingUser = await repository.GetById(id);
        if (existingUser is null)
        {
            return Result<UserDto?>.NotFound();
        }

        var success = await repository.Delete(id);

        if (success)
        {
            await busRepository.Publish(new UserDeletedMessage
            {
                UserId = id
            });
        }
        
        return success
            ? new Result<UserDto?>(mapper.Map<UserDto?>(existingUser))
            : Result<UserDto?>.Fail("Failed to delete user.");
    }
}