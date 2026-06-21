// CompanyFaker.csx
#nuget "Bogus, 35.6.5"

#load "Company.csx"

public class CompanyFaker : Bogus.Faker<Company>
{
    public CompanyFaker() : base("sk")
    {
        RuleFor(c => c.Name, f => f.Company.CompanyName());
        RuleFor(c => c.Code, f => f.Random.AlphaNumeric(6).ToUpper());
    }
}