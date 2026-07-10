namespace APIKros.Models;

public class Project : HierarchyNode
{
    public int DivisionId { get; private set; }

    public Division Division { get; private set; } = null!;

    public ICollection<Department> Departments { get; private set; } = new List<Department>();

    private Project() { } // EF Core

    public Project(
        string name,
        string code,
        int divisionId,
        int? managerId = null)
        : base(name, code, managerId)
    {
        DivisionId = divisionId;
    }

    public void AddDepartment(Department department)
    {
        Departments.Add(department);
    }
    public void ChangeDivision(int divisionId)
    {
        DivisionId = divisionId;
    }
}