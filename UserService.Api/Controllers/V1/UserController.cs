using Microsoft.AspNetCore.Mvc;
using UserService.Application.Services;

namespace UserService.Controllers.V1;

[ApiController]
[Route("v1/users")]
public class UserController(IUserService userService) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetByIds(
        [FromQuery] List<string> ids
    )
    {
        var usersResult = await userService.GetByIds(ids);
        return HandleResult(usersResult);
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