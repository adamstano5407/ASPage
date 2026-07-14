using System.ComponentModel.DataAnnotations;

namespace APIKros.Models;

public abstract class HierarchyNode : IModel<int>
{
    public int Id { get; private set; }

    [Required]
    public string Name { get; private set; } = null!;

    [Required]
    public string Code { get; private set; } = null!;

    public int? ManagerId { get; private set; }
    public Employee? Manager { get; private set; }

    protected HierarchyNode() { }

    protected HierarchyNode(
        string name,
        string code,
        int? managerId = null)
    {
        Name = name;
        Code = code;
        ManagerId = managerId;
    }

    public void ChangeName(string name) => Name = name;

    public void ChangeCode(string code) => Code = code;

    public void AssignManager(int? managerId)
    {
        ManagerId = managerId;
    }
}