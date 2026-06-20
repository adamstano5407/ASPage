using APIKros.DTOs.Employee;
using APIKros.DTOs.Project;

namespace APIKros.DTOs.Department
{
    public class DetailedDepartmentDto : HierarchyNodeDto, IDto<Models.Department, DetailedDepartmentDto>
    {
        public EmployeeDto? Manager { get; set; }
        

        public static DetailedDepartmentDto CreateInstance(Models.Department department)
        {
            return new DetailedDepartmentDto
            {
                Id = department.Id,
                Name = department.Name,
                Code = department.Code,
                Manager = department.Manager is null
                    ? null
                    : EmployeeDto.CreateInstance(department.Manager),
            };
        }
    }

}

