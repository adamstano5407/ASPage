using APIKros.DTOs.Auth;
using Microsoft.AspNetCore.Identity.Data;
using LoginRequest = APIKros.Requests.Auth.LoginRequest;

namespace APIKros.Services.Auth;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RefreshAsync(string refreshToken);
    Task LogoutAsync(string refreshToken);
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
}

public class AuthService : IAuthService
{
    public Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<AuthResponse> RefreshAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }

    public Task LogoutAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }

    public Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        throw new NotImplementedException();
    }
}