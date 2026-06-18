namespace APIKros.DTOs;

public abstract class HierarchyNodeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public int? ManagerId { get; set; }
}