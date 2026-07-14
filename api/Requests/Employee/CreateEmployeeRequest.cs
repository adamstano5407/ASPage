using System.ComponentModel.DataAnnotations;

namespace APIKros.Requests.Employee
{
    public class CreateEmployeeRequest
    {
        [Required] 
        public string EmployeeNumber { get; set; } = null!;
        
        public string? Title { get; set; } = null;
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Phone { get; set; } = null!;
        [Required]
        public int CompanyId { get; set; }
    }

}