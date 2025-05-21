using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Projeli.UserService.Application.Dtos;
using Projeli.UserService.Application.Models.Requests;
using Projeli.UserService.Application.Services;

namespace Projeli.UserService.Api.Controllers.V1;

[ApiController]
[Route("v1/users")]
public class UserController(IUserService userService, IMapper mapper) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetUsers(
        [FromQuery] List<string>? ids = null,
        [FromQuery] string? username = null
    )
    {
        if (ids is { Count: > 0 })
        {
            var usersResult = await userService.GetByIds(ids);
            return HandleResult(usersResult);
        }

        if (!string.IsNullOrWhiteSpace(username))
        {
            var searchResult = await userService.SearchByUsername(username);
            return HandleResult(searchResult);
        }

        return ValidationProblem(new ValidationProblemDetails
            {
                Errors = new Dictionary<string, string[]>
                {
                    { "ids", ["ids cannot be empty"] },
                    { "username", ["username cannot be empty"] }
                }
            }
        );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        [FromRoute] string id
    )
    {
        var userResult = await userService.GetById(id);
        return HandleResult(userResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var userResult = await userService.Create(mapper.Map<UserDto>(request));
        return HandleResult(userResult);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        [FromRoute] string id,
        [FromBody] UpdateUserRequest request
    )
    {
        var userResult = await userService.Update(id, mapper.Map<UserDto>(request));
        return HandleResult(userResult);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        [FromRoute] string id
    )
    {
        var userResult = await userService.Delete(id);
        return HandleResult(userResult);
    }
}