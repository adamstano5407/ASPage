using System.ComponentModel.DataAnnotations;

namespace APIKros.Models { 
public class Division
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Code { get; set; } = null!;

        public int CompanyId { get; set; }

        public Company Company { get; set; } = null!;

        public int ManagerId { get; set; } 

        public Employee Manager { get; set; } = null!;

        public ICollection<Project> Projects { get; set; } = new List<Project>();

    
    }
}