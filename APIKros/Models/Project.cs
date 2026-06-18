

using System.ComponentModel.DataAnnotations;

namespace APIKros.Models
{
    
    public class Project : HierarchyNode
{
    public int DivisionId { get; set; }
    public Division Division { get; set; } = null!;
    
    public ICollection<Department> Departments { get; set; } = new List<Department>();
}
}

