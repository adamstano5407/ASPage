using System.ComponentModel.DataAnnotations;

namespace APIKros.Models { 
public class Division : HierarchyNode
    {
        public int CompanyId { get; set; }

        public Company Company { get; set; } = null!;

        public ICollection<Project> Projects { get; set; } = new List<Project>();

    
    }
}