// Seeders/Fakers/ProjectFaker.cs
using APIKros.Models;
using Bogus;

namespace APIKros.Seeders.Fakers;

public class ProjectFaker : Faker<Project>
{
    public ProjectFaker(int divisionId, List<Employee> employees) : base("sk")
    {
        RuleFor(p => p.Name, f => f.Commerce.ProductName());
        RuleFor(p => p.Code, f => f.Random.AlphaNumeric(5).ToUpper());
        RuleFor(p => p.DivisionId, divisionId);
        RuleFor(d => d.ManagerId, f => employees[f.Random.Int(0, employees.Count - 1)].Id);
    }
}