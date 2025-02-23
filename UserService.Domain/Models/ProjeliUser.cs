using System.ComponentModel.DataAnnotations;

namespace UserService.Domain.Models;

public class ProjeliUser
{
    public string Id { get; set; }
    
    public string UserName { get; set; }
    
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }
    
    public string? Email { get; set; }
    
    public string? ImageUrl { get; set; }
}