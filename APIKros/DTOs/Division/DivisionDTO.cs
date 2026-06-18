namespace APIKros.DTOs.Division
{
    public class DivisionDto : HierarchyNodeDto,IDto<Models.Division, DivisionDto>
    {
        public int CompanyId { get; set; }
       
        public static DivisionDto CreateInstance(Models.Division division)
        {
            return new DivisionDto
            {
                Id = division.Id,
                Name = division.Name,
                Code = division.Code,
                CompanyId = division.CompanyId,
                ManagerId = division.ManagerId
            };
        }
    }

}