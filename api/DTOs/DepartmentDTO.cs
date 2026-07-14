namespace APIKros.DTOs
{
    public class DepartmentDto : HierarchyNodeDto, IDto
    {
        public int ProjectId { get; private set; }
        
        public DepartmentDto(
            int id,
            string name,
            string code,
            int? managerId,
            EmployeeDto? manager,
            int projectId)
            : base(id, name, code, managerId, manager)
        {
            ProjectId = projectId;
        }
    }
}