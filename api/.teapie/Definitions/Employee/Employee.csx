#load ../IClone.csx

public class Employee : IClone<Employee>
{
    public int? Id { get; set; }
    public string EmployeeNumber { get; set; } = "";
    public string? Title { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public int CompanyId { get; set; }

    public override string ToString()
    {
        return $"Id: {Id}, " +
               $"EmployeeNumber: {EmployeeNumber}, " +
               $"Title: {Title}, " +
               $"FirstName: {FirstName}, " +
               $"LastName: {LastName}, " +
               $"Email: {Email}, " +
               $"Phone: {Phone}, " +
               $"CompanyId: {CompanyId}";
    }

    public Employee Clone()
    {
        return new Employee
        {
            Id = Id,
            EmployeeNumber = EmployeeNumber,
            Title = Title,
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            Phone = Phone,
            CompanyId = CompanyId
        };
    }
}