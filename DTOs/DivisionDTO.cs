
namespace APIKros.DTOs
{
    public class DivisionDto : HierarchyNodeDto, IDto
    {
        public int CompanyId { get; private set; }

        public DivisionDto(
            int id,
            string name,
            string code,
            int? managerId,
            EmployeeDto? manager,
            int companyId)
            : base(id, name, code, managerId, manager)
        {
            CompanyId = companyId;
        }
    }
}