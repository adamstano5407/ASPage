namespace APIKros.DTOs.Employee;

public class EmployeeDto : IDto<Models.Employee, EmployeeDto>
{
    public int Id { get; set; }
    
    public string EmployeeNumber { get; set; } = null!;
    public string? Title { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public int CompanyId { get; set; } 

    public static EmployeeDto CreateInstance(Models.Employee employee)
    {
        return new EmployeeDto
        {
            Id = employee.Id,
            Title = employee.Title,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            Phone = employee.Phone,
            EmployeeNumber =  employee.EmployeeNumber,
            CompanyId = employee.CompanyId
        };
    }
}