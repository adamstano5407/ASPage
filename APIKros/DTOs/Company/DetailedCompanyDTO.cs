using APIKros.DTOs.Division;
using APIKros.DTOs.Employee;

namespace APIKros.DTOs.Company
{
    public class DetailedCompanyDto : HierarchyNodeDto, IDto<Models.Company, DetailedCompanyDto>
    {
        public EmployeeDto? Manager { get; set; }
        public List<DivisionDto> Divisions { get; set; } = new();
        public List<EmployeeDto> Employees { get; set; } = new();

        public static DetailedCompanyDto CreateInstance(Models.Company company)
        {
            return new DetailedCompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                Code = company.Code,
                Manager = company.Manager is null
                    ? null
                    : EmployeeDto.CreateInstance(company.Manager),
                Divisions = company.Divisions
                    .Select(DivisionDto.CreateInstance)
                    .ToList(),
                Employees = company.Employees
                    .Select(EmployeeDto.CreateInstance)
                    .ToList()
            };
        }
    }

}