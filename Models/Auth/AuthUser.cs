namespace APIKros.Models.Auth;

public class AuthUser : IModel<int>
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpires { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = [];
}