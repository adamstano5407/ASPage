namespace APIKros.DTOs.Company
{
    public class CompanyDto : HierarchyNodeDto, IDto<Models.Company, CompanyDto>
    {
        public static CompanyDto CreateInstance(Models.Company company)
        {
            return new CompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                Code = company.Code,
                ManagerId = company.ManagerId
            };
        }
    }

}