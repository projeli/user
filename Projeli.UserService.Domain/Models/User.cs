using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Projeli.UserService.Domain.Models;

[Index(nameof(UserId), IsUnique = true)]
[Index(nameof(UserName), IsUnique = true)]
public class User
{
    [Key]
    public Ulid Id { get; set; }
    
    [Required, MaxLength(32)]
    public string UserId { get; set; }

    [Required, MaxLength(32)]
    public string UserName { get; set; }

    [StringLength(128)]
    public string? FirstName { get; set; }

    [StringLength(128)]
    public string? LastName { get; set; }

    [Required, MaxLength(256)]
    public string Email { get; set; }

    [Required, MaxLength(256)]
    public string ImageUrl { get; set; }
}