using System.ComponentModel.DataAnnotations;

namespace APIKros.Models
{

    public class Department : HierarchyNode
    {
        public int ProjectId { get; set; }
        public Project? Project { get; set; }
    }
}

