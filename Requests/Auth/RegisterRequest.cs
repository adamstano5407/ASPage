using System.ComponentModel.DataAnnotations;

namespace APIKros.Requests.Auth;

public class RegisterRequest
{
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
}