namespace APIKros.DTOs.Auth;

public class AuthUserDto
{
    public int Id { get; private set; }
    public string Email { get; private set; } = null!;
    public ICollection<string> Roles { get; private set; } = [];
    
    private AuthUserDto()
    {
        Email = null!;
        Roles = [];
    }

    public AuthUserDto(
        int id,
        string email,
        ICollection<string> roles)
    {

        Id = id;
        Email = email;
        Roles = roles;
    } 
}