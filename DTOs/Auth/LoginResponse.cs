namespace APIKros.DTOs.Auth;

public class LoginResponse
{
    public string AccessToken { get; set; } = null!;
    public AuthUserDto User { get; set; } = null!;
}