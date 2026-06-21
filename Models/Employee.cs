
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIKros.Models
{


    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required] 
        public string EmployeeNumber = null!;
        
        public string? Title { get; set; } = null;
        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        public string Phone { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        
        [Required]
        public int CompanyId { get; set; }

        public Company Company { get; set; } = null!;


    }
}