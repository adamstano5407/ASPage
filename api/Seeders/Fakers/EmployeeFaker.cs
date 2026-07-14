using APIKros.Models;
using Bogus;

namespace APIKros.Seeders.Fakers;

public sealed class EmployeeFaker : Faker<Employee>
{
    public EmployeeFaker(int companyId) : base("sk")
    {
        CustomInstantiator(f => new Employee(
            employeeNumber: $"EMP-{f.UniqueIndex:D5}",
            title: f.PickRandom<string?>(new[]
            {
                "Ing.",
                "Mgr.",
                "Bc.",
                "PhD.",
                "MBA",
                null
            }),
            firstName: f.Name.FirstName(),
            lastName: f.Name.LastName(),
            email: f.Internet.Email(),
            phone: f.Phone.PhoneNumber(),
            companyId: companyId
        ));
    }
}