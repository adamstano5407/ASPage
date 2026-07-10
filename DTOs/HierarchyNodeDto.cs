
namespace APIKros.DTOs;

public abstract class HierarchyNodeDto
{
    public int Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Code { get; private set; } = null!;
    public int? ManagerId { get; private set; }
    public EmployeeDto? Manager { get; private set; }

    protected HierarchyNodeDto() { }

    protected HierarchyNodeDto(
        int id,
        string name,
        string code,
        int? managerId,
        EmployeeDto? manager)
    {
        Id = id;
        Name = name;
        Code = code;
        ManagerId = managerId;
        Manager = manager;
    }
}