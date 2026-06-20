using APIKros.DTOs.Company;
using APIKros.DTOs.Employee;
using APIKros.DTOs.Project;

namespace APIKros.DTOs.Division
{
    public class DetailedDivisionDto : HierarchyNodeDto, IDto<Models.Division, DetailedDivisionDto>
    {
      
        public EmployeeDto? Manager { get; set; }
        public List<ProjectDto> Projects { get; set; } = new();

        public static DetailedDivisionDto CreateInstance(Models.Division division)
        {
            return new DetailedDivisionDto
            {
                Id = division.Id,
                Name = division.Name,
                Code = division.Code,
                Manager = division.Manager is null
                    ? null
                    : EmployeeDto.CreateInstance(division.Manager),
                Projects = division.Projects
                    .Select(ProjectDto.CreateInstance)
                    .ToList(),
            };
        }
    }
}