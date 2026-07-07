namespace APIKros.DTOs;

public class ProjectDto : HierarchyNodeDto ,IDto<Models.Project, ProjectDto>
{
    public int DivisionId { get; set; }
    public static ProjectDto CreateInstance(Models.Project project)
    {
        return new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            Code = project.Code,
            DivisionId = project.DivisionId,
            ManagerId = project.ManagerId, 
            Manager = project.Manager == null ? null : EmployeeDto.CreateInstance(project.Manager)
        };
    }
}