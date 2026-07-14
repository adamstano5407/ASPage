namespace APIKros.Models;

public class Employee : IModel<int>
{
    public int Id { get; private set; }

    public string EmployeeNumber { get; private set; } = null!;
    public string? Title { get; private set; }
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string Phone { get; private set; } = null!;
    public string Email { get; private set; } = null!;

    public int CompanyId { get; private set; }
    public Company Company { get; private set; } = null!;

    private Employee()
    {
    } // EF Core

    public Employee(
        string employeeNumber,
        string? title,
        string firstName,
        string lastName,
        string phone,
        string email,
        int companyId)
    {
        EmployeeNumber = employeeNumber;
        Title = title;
        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
        Email = email;
        CompanyId = companyId;
    }

    public void ChangePhone(string phone) => Phone = phone;

    public void ChangeEmail(string email) => Email = email;

    public void ChangeTitle(string? title) => Title = title;

    public void ChangeCompany(int companyId) => CompanyId = companyId;
    
    public void ChangeFirstName(string firstName) => FirstName = firstName;
    
    public void ChangeLastName(string lastName) => LastName = lastName;
    
    public void ChangeEmployeeNumber(string employeeNumber) => EmployeeNumber = employeeNumber;

}