namespace APIKros.DTOs;

public class EmployeeDto : IDto
{
    public int Id { get; set; }
    
    public string EmployeeNumber { get; set; } = null!;
    public string? Title { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public int CompanyId { get; set; } 

    
}