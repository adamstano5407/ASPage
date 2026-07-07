namespace APIKros.DTOs
{
    public class DepartmentDto : HierarchyNodeDto, IDto<Models.Department, DepartmentDto>
    {
        public int ProjectId { get; set; }
        
        public static DepartmentDto CreateInstance(Models.Department department)
        {
            return new DepartmentDto
            {
                Id = department.Id,
                Name = department.Name,
                Code = department.Code,
                ProjectId = department.ProjectId,
                ManagerId = department.ManagerId,
                Manager = department.Manager is null
                    ? null
                    : EmployeeDto.CreateInstance(department.Manager),
            };
        }
    }
}