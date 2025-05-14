using Clerk.BackendAPI;
using Clerk.BackendAPI.Models.Operations;
using UserService.Domain.Models;
using UserService.Domain.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Clerk.BackendAPI.Models.Errors;

namespace UserService.Infrastructure.Repositories;

public class UserRepository(IClerkBackendApi clerkBackendApi, IDistributedCache cache) : IUserRepository
{
    private readonly DistributedCacheEntryOptions _cacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
    };

    public async Task<List<ProjeliUser>?> GetByIds(List<string> ids)
    {
        if (ids.Count == 0)
        {
            return [];
        }

        var result = new List<ProjeliUser>();
        var usersToFetch = new List<string>();

        // Check cache for each user individually
        foreach (var id in ids)
        {
            var cacheKey = $"user_{id}";
            string? cachedData = null!;
            // var cachedData = await cache.GetStringAsync(cacheKey);

            if (cachedData != null)
            {
                var user = JsonSerializer.Deserialize<ProjeliUser>(cachedData);
                if (user != null)
                {
                    result.Add(user);
                    continue;
                }
            }

            usersToFetch.Add(id);
        }

        // Fetch missing users from Clerk
        if (usersToFetch.Count != 0)
        {
            var request = new GetUserListRequest
            {
                UserId = usersToFetch,
            };

            try
            {
                var users = await clerkBackendApi.Users.ListAsync(request);

                if (users.UserList != null)
                {
                    foreach (var user in users.UserList)
                    {
                        var projeliUser = new ProjeliUser
                        {
                            Id = user.Id!,
                            UserName = user.Username!,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.EmailAddresses
                                ?.FirstOrDefault(y => y.Id?.Equals(user.PrimaryEmailAddressId) ?? false)
                                ?.EmailAddressValue,
                            ImageUrl = user.ImageUrl
                        };

                        result.Add(projeliUser);

                        // Store each user individually in cache
                        var cacheKey = $"user_{projeliUser.Id}";
                        await cache.SetStringAsync(
                            cacheKey,
                            JsonSerializer.Serialize(projeliUser),
                            _cacheOptions);
                    }
                }
            }
            catch (Exception ex) when (ex is ClerkErrors or SDKError)
            {
                return null;
            }
        }

        return result;
    }

    public async Task<ProjeliUser?> GetById(string id)
    {
        var cacheKey = $"user_{id}";
        var cachedData = await cache.GetStringAsync(cacheKey);

        if (cachedData != null)
        {
            return JsonSerializer.Deserialize<ProjeliUser>(cachedData);
        }

        try
        {
            var user = await clerkBackendApi.Users.GetAsync(userId: id);

            if (user.User == null)
            {
                return null;
            }

            var result = new ProjeliUser
            {
                Id = user.User.Id!,
                UserName = user.User.Username!,
                FirstName = user.User.FirstName,
                LastName = user.User.LastName,
                Email = user.User.EmailAddresses
                    ?.FirstOrDefault(x => x.Id?.Equals(user.User.PrimaryEmailAddressId) ?? false)?.EmailAddressValue,
                ImageUrl = user.User.ImageUrl
            };

            await cache.SetStringAsync(
                cacheKey,
                JsonSerializer.Serialize(result),
                _cacheOptions);

            return result;
        }
        catch (Exception ex) when (ex is ClerkErrors or SDKError)
        {
            return null;
        }
    }
}