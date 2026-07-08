namespace APIKros.DTOs.Auth;

public class AuthUserDto
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public ICollection<string> Roles { get; set; } = [];
}