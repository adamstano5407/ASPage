namespace APIKros.DTOs.Auth;

public class AccessTokenDto
{
    public string Token { get; }

    public DateTime ExpiresAt { get; }

    public AccessTokenDto(string token, DateTime expiresAt)
    {
        Token = token;
        ExpiresAt = expiresAt;
    }
}