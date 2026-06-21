// DivisionFaker.csx
#nuget "Bogus, 35.6.5"

#load "Division.csx"

public class DivisionFaker : Bogus.Faker<Division>
{
    public DivisionFaker() : base("sk")
    {
        RuleFor(d => d.Name, f => f.Commerce.Department());
        RuleFor(d => d.Code, f => f.Random.AlphaNumeric(5).ToUpper());
        RuleFor(d => d.CompanyId, 1);
        RuleFor(d => d.ManagerId, 1);
    }
}