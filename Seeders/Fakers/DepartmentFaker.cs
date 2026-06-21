// Seeders/Fakers/DepartmentFaker.cs
using APIKros.Models;
using Bogus;

namespace APIKros.Seeders.Fakers;

public class DepartmentFaker : Faker<Department>
{
    public DepartmentFaker(int projectId, List<Employee> employees) : base("sk")
    {
        RuleFor(d => d.Name, f => f.Commerce.Department());
        RuleFor(d => d.Code, f => f.Random.AlphaNumeric(5).ToUpper());
        RuleFor(d => d.ProjectId, projectId);
        RuleFor(d => d.ManagerId, f => f.PickRandom(employees).Id);
    }
}