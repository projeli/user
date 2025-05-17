using Microsoft.AspNetCore.Mvc;
using UserService.Application.Services;

namespace UserService.Controllers.V1;

[ApiController]
[Route("v1/users")]
public class UserController(IUserService userService) : BaseController
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
}