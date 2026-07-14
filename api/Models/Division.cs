namespace APIKros.Models;

public class Division : HierarchyNode
{
    public int CompanyId { get; private set; }

    public Company Company { get; private set; } = null!;

    public ICollection<Project> Projects { get; private set; } = new List<Project>();

    private Division() { } // EF Core

    public Division(
        string name,
        string code,
        int companyId,
        int? managerId = null)
        : base(name, code, managerId)
    {
        CompanyId = companyId;
    }

    public void ChangeCompany(int companyId)
    {
        CompanyId = companyId;
    }
}