// Seeders/Fakers/DepartmentFaker.cs
using APIKros.Models;
using Bogus;

namespace APIKros.Seeders.Fakers;

public class DepartmentFaker : Faker<Department>
{
    public DepartmentFaker(int projectId, List<Employee> employees) : base("sk")
    {
        CustomInstantiator(f =>
        {
            var managerId = employees.Count > 0
                ? f.PickRandom(employees).Id
                : (int?)null;

            return new Department(
                name: f.Commerce.Department(),
                code: f.Random.AlphaNumeric(5).ToUpperInvariant(),
                projectId: projectId,
                managerId: managerId
            );
        });
    }
}