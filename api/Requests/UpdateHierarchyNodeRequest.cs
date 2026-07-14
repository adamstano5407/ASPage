namespace APIKros.Requests;

public class UpdateHierarchyNodeRequest
{
    public int Id { get; set; }
    public string? Name { get; set; } = null!;
    public string? Code { get; set; } = null!;
        
    public int? ManagerId { get; set; }
    
}

public class UpdateCompanyRequest : UpdateHierarchyNodeRequest {}
public class UpdateDivisionRequest : UpdateHierarchyNodeRequest {}
public class UpdateProjectRequest : UpdateHierarchyNodeRequest {}
public class UpdateDepartmentRequest : UpdateHierarchyNodeRequest {}