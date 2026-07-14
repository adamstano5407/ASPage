using System.ComponentModel.DataAnnotations;

namespace APIKros.Models;

public class Department : HierarchyNode
{
    public int ProjectId { get; private set; }

    public Project Project { get; private set; } = null!;
    
    private Department()
    {
        // Pre EF Core
    }

    public Department(
        string name,
        string code,
        int projectId,
        int? managerId = null)
        : base(name, code, managerId)
    {
        ProjectId = projectId;
    }

    public void ChangeProject(int projectId)
    {
        ProjectId = projectId;
    }
}