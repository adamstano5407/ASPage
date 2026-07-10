namespace APIKros.DTOs;
public class EmployeeDto : IDto
{
    public int Id { get; private set; }

    public string EmployeeNumber { get; private set; }
    public string? Title { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }
    public int CompanyId { get; private set; }

    public EmployeeDto(
        int id,
        string employeeNumber,
        string? title,
        string firstName,
        string lastName,
        string email,
        string phone,
        int companyId)
    {
        Id = id;
        EmployeeNumber = employeeNumber;
        Title = title;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        CompanyId = companyId;
    }
}