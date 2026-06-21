using APIKros.Models;
using Bogus;

namespace APIKros.Seeders.Fakers;

public class EmployeeFaker : Faker<Employee>
{
    public EmployeeFaker(int companyId) : base("sk")
    {
        RuleFor(e => e.EmployeeNumber, f => $"EMP-{f.UniqueIndex:D5}");
        RuleFor(e => e.Title, f =>
            f.PickRandom<string?>(new[]
            {
                "Ing.",
                "Mgr.",
                "Bc.",
                "PhD.",
                "MBA",
                null
            }));
        RuleFor(e => e.FirstName, f => f.Name.FirstName());
        RuleFor(e => e.LastName, f => f.Name.LastName());
        RuleFor(e => e.Email, f => f.Internet.Email());
        RuleFor(e => e.Phone, f => f.Phone.PhoneNumber());
        RuleFor(e => e.CompanyId, companyId);
    }
}