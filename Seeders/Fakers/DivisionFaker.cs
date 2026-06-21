// Seeders/Fakers/DivisionFaker.cs
using APIKros.Models;
using Bogus;

namespace APIKros.Seeders.Fakers;

public class DivisionFaker : Faker<Division>
{
    public DivisionFaker(int companyId, List<Employee> employees) : base("sk")
    {
        RuleFor(d => d.Name, f => f.Commerce.Department());
        RuleFor(d => d.Code, f => f.Random.AlphaNumeric(5).ToUpper());
        RuleFor(d => d.CompanyId, companyId);
        RuleFor(d => d.ManagerId, f => employees[f.Random.Int(0, employees.Count - 1)].Id);
    }
}