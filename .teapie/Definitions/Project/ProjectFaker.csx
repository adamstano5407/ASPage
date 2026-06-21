// ProjectFaker.csx
#nuget "Bogus, 35.6.5"

#load "Project.csx"

public class ProjectFaker : Bogus.Faker<Project>
{
    public ProjectFaker() : base("sk")
    {
        RuleFor(p => p.Name, f => f.Commerce.ProductName());
        RuleFor(p => p.Code, f => f.Random.AlphaNumeric(5).ToUpper());
        RuleFor(p => p.DivisionId, 1);
        RuleFor(p => p.ManagerId, 1);
    }
}