using System.ComponentModel.DataAnnotations;

namespace APIKros.Requests;

public class CreateHierarchyNodeRequest
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Code { get; set; } = null!;

    public int? ManagerId { get; set; }
}

public class CreateNonRootHierarchyNodeRequest : CreateHierarchyNodeRequest
{
    [Required]
    public int ParentId { get; set; }
}

public class CreateCompanyRequest : CreateHierarchyNodeRequest {}

public class CreateDivisionRequest : CreateNonRootHierarchyNodeRequest {}

public class CreateProjectRequest : CreateNonRootHierarchyNodeRequest {}

public class CreateDepartmentRequest : CreateNonRootHierarchyNodeRequest {}