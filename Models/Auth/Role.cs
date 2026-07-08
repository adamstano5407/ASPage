namespace APIKros.Models.Auth;

public class Role : IModel<int>
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ICollection<UserRole> UserRoles { get; set; } = [];
    public int Id { get; set; }
}