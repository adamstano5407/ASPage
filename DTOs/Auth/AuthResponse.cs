namespace APIKros.DTOs.Auth;

public class AuthResponse
{
    public string AccessToken { get; set; } = string.Empty;
    
    public AuthUserDto User { get; set; } = new();
}