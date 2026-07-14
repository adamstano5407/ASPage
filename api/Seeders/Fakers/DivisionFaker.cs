// Seeders/Fakers/DivisionFaker.cs
using APIKros.Models;
using Bogus;

namespace APIKros.Seeders.Fakers;

public class DivisionFaker : Faker<Division>
{
    public DivisionFaker(
        int companyId,
        List<Employee> employees)
        : base("sk")
    {

        CustomInstantiator(f => new Division(
            name: f.Commerce.Department(),
            code: f.Random.AlphaNumeric(5).ToUpperInvariant(),
            companyId: companyId,
            managerId: f.PickRandom(employees).Id
        ));
    }
}