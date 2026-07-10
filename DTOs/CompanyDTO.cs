namespace APIKros.DTOs
{
    public class CompanyDto : HierarchyNodeDto, IDto
    {
        public CompanyDto(
            int id,
            string name,
            string code,
            int? managerId,
            EmployeeDto? manager)
            : base(id, name, code, managerId, manager)
        {
        }
    }
}