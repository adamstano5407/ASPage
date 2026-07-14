using Azure.Core;

namespace APIKros.DTOs.Auth;

public class LoginResponse
{
    public string AccessToken { get; private set; }
    public AuthUserDto User { get; private  set; }

    public LoginResponse(
        string accessToken,
        AuthUserDto user)
    {
        AccessToken = accessToken;
        User = user;
    }
}