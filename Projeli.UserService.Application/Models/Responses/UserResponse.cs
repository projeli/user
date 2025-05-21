namespace Projeli.UserService.Application.Models.Responses;

public class UserResponse
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string ImageUrl { get; set; }
}