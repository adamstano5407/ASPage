public abstract class HierarchyNodeResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public int? ManagerId { get; set; }

    public EmployeeResponse? Manager { get; set; }
}

public class CompanyResponse : HierarchyNodeResponse
{
}

public class DivisionResponse : HierarchyNodeResponse
{
    public int CompanyId { get; set; }
}

public class ProjectResponse : HierarchyNodeResponse
{
    public int DivisionId { get; set; }
}

public class DepartmentResponse : HierarchyNodeResponse
{
    public int ProjectId { get; set; }
}

public class EmployeeResponse
{
    public int Id { get; set; }

    public string EmployeeNumber { get; set; } = string.Empty;

    public string? Title { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public int CompanyId { get; set; }
}