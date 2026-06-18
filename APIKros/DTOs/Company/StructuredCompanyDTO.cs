using APIKros.DTOs.Division;
using APIKros.DTOs.Employee;

namespace APIKros.DTOs.Company
{
    public class StructuredCompanyDto : HierarchyNodeDto, IDto<Models.Company, StructuredCompanyDto>
    {
       
        public EmployeeDto? Director { get; set; }
        public List<StructuredDivisionDto> Divisions { get; set; } = new();
        public List<EmployeeDto> Employees { get; set; } = new();

        public static StructuredCompanyDto CreateInstance(Models.Company company)
        {
            return new StructuredCompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                Code = company.Code,
                Director = company.Manager is null
                    ? null
                    : EmployeeDto.CreateInstance(company.Manager),
                Divisions = company.Divisions
                    .Select(StructuredDivisionDto.CreateInstance)
                    .ToList(),
                Employees = company.Employees
                    .Select(EmployeeDto.CreateInstance)
                    .ToList()
            };
        }
    }


}