using MassTransit;
using Microsoft.EntityFrameworkCore;
using Projeli.Shared.Application.Messages.Users;
using Projeli.UserService.Infrastructure.Database;
using Projeli.UserService.Domain.Models;
using Projeli.UserService.Domain.Repositories;

namespace Projeli.UserService.Infrastructure.Repositories;

public class UserRepository(UserServiceDbContext database) : IUserRepository
{
    public async Task<List<User>> GetByIds(List<string> ids)
    {
        var users = await database.Users
            .Where(user => ids.Contains(user.UserId))
            .Select(user => new User
            {
                Id = user.Id,
                UserId = user.UserId,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ImageUrl = user.ImageUrl
            })
            .ToListAsync();

        return users;
    }

    public async Task<List<User>> SearchByUsername(string username)
    {
        var users = await database.Users
            .Where(user => user.UserName.Contains(username))
            .Select(user => new User
            {
                Id = user.Id,
                UserId = user.UserId,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ImageUrl = user.ImageUrl
            })
            .ToListAsync();

        return users;
    }

    public async Task<User?> GetById(string id)
    {
        var user = await database.Users
            .Where(user => user.UserId == id)
            .Select(user => new User
            {
                Id = user.Id,
                UserId = user.UserId,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ImageUrl = user.ImageUrl
            })
            .FirstOrDefaultAsync();

        return user;
    }

    public async Task<User?> Create(User user)
    {
        var newUser = new User
        {
            Id = user.Id,
            UserId = user.UserId,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            ImageUrl = user.ImageUrl
        };

        await database.Users.AddAsync(newUser);
        await database.SaveChangesAsync();

        return newUser;
    }

    public async Task<User?> Update(string userId, User user)
    {
        var existingUser = await database.Users
            .Where(u => u.UserId == userId)
            .FirstOrDefaultAsync();

        if (existingUser is null)
        {
            return null;
        }

        existingUser.UserName = user.UserName;
        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;
        existingUser.Email = user.Email;
        existingUser.ImageUrl = user.ImageUrl;

        await database.SaveChangesAsync();

        return existingUser;
    }

    public async Task<bool> Delete(string id)
    {
        var user = await database.Users
            .Where(user => user.UserId == id)
            .FirstOrDefaultAsync();

        if (user is null) return false;

        database.Users.Remove(user);
        
        return await database.SaveChangesAsync() > 0;
    }
}