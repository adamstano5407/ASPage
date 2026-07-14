// Seeders/Fakers/ProjectFaker.cs

using APIKros.Models;
using Bogus;

namespace APIKros.Seeders.Fakers;

public class ProjectFaker : Faker<Project>
{
    public ProjectFaker(
        int divisionId,
        List<Employee> employees)
        : base("sk")
    {
        if (employees.Count == 0)
        {
            throw new ArgumentException(
                "Project faker requires at least one employee.",
                nameof(employees));
        }

        CustomInstantiator(f => new Project(
            name: f.Commerce.ProductName(),
            code: f.Random.AlphaNumeric(5).ToUpperInvariant(),
            divisionId: divisionId,
            managerId: f.PickRandom(employees).Id
        ));
    }
}