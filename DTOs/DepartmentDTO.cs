namespace APIKros.DTOs
{
    public class DepartmentDto : HierarchyNodeDto, IDto
    {
        public int ProjectId { get; set; }
        
    }
}