using APIKros.Models;
using Bogus;

namespace APIKros.Seeders.Fakers;

public class CompanyFaker : Faker<Company>
{
    public CompanyFaker() : base("sk")
    {
        RuleFor(c => c.Name, f => f.Company.CompanyName());
        RuleFor(c => c.Code, f => f.Random.AlphaNumeric(6).ToUpper());
    }
}