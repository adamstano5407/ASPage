using APIKros.Models;
using Bogus;

namespace APIKros.Seeders.Fakers;

public sealed class CompanyFaker : Faker<Company>
{
    public CompanyFaker() : base("sk")
    {
        CustomInstantiator(f => new Company(
            f.Company.CompanyName(),
            f.Random.AlphaNumeric(6).ToUpperInvariant(),
            managerId: null
        ));
    }
}