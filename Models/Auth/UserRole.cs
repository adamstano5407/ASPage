namespace APIKros.Models.Auth;

public class UserRole
{
    public int UserId { get; set; }
    public int RoleId { get; set; }
    public DateTime AsssignDate { get; set; }
    public int? AssignedByUserId { get; set; }
    public AuthUser User { get; set; } = null!;
    public Role Role { get; set; } = null!;
}