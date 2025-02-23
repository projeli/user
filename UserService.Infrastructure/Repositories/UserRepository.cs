using Clerk.BackendAPI;
using Clerk.BackendAPI.Models.Operations;
using UserService.Domain.Models;
using UserService.Domain.Repositories;

namespace UserService.Infrastructure.Repositories;

public class UserRepository(IClerkBackendApi clerkBackendApi) : IUserRepository
{
    public async Task<List<ProjeliUser>?> GetByIds(List<string> ids)
    {
        if (ids.Count == 0)
        {
            return [];
        }
        
        var request = new GetUserListRequest
        {
            UserId = ids,
        };
        
        var users = await clerkBackendApi.Users.ListAsync(request);
        
        return users.UserList?.Select(x => new ProjeliUser
        {
            Id = x.Id!,
            UserName = x.Username!,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Email = x.EmailAddresses?.FirstOrDefault(y => y.Id?.Equals(x.PrimaryEmailAddressId) ?? false)?.EmailAddressValue,
            ImageUrl = x.ImageUrl
        }).ToList();
    }

    public async Task<ProjeliUser?> GetById(string id)
    {
        var user = await clerkBackendApi.Users.GetAsync(userId: id);
        
        return user.User == null ? null : new ProjeliUser
        {
            Id = user.User.Id!,
            UserName = user.User.Username!,
            FirstName = user.User.FirstName,
            LastName = user.User.LastName,
            Email = user.User.EmailAddresses?.FirstOrDefault(x => x.Id?.Equals(user.User.PrimaryEmailAddressId) ?? false)?.EmailAddressValue,
            ImageUrl = user.User.ImageUrl
        };
    }
}