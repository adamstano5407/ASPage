using APIKros.DTOs.Company;
using APIKros.DTOs.Employee;
using APIKros.DTOs.Project;

namespace APIKros.DTOs.Division
{

    public class StructuredDivisionDto : HierarchyNodeDto,IDto<Models.Division, StructuredDivisionDto>
    {
        public EmployeeDto? Manager { get; set; }

        public List<DetailedProjectDto> Projects { get; set; } = new();
        public static StructuredDivisionDto CreateInstance(Models.Division division)
        {
            return new StructuredDivisionDto
            {
                Id = division.Id,
                Name = division.Name,
                Code = division.Code,
                Manager = division.Manager is null
                    ? null
                    : EmployeeDto.CreateInstance(division.Manager),
                Projects = division.Projects.Select(DetailedProjectDto.CreateInstance).ToList()
            };
        }
    }
}
