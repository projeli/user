using Microsoft.AspNetCore.Mvc;

namespace UserService.Controllers;

[ApiController]
[Route("users")]
public class UserController: ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { message = "Hello from UserService" });
    }
}