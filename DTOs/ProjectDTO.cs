
namespace APIKros.DTOs;


public class ProjectDto : HierarchyNodeDto, IDto
{
    public int DivisionId { get; private set; }

    public ProjectDto(
        int id,
        string name,
        string code,
        int? managerId,
        EmployeeDto? manager,
        int divisionId)
        : base(id, name, code, managerId, manager)
    {
        DivisionId = divisionId;
    }
}