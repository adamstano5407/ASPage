#nuget "Bogus, 35.6.5"

#load "Employee.csx"

public class EmployeeFaker : Bogus.Faker<Employee>
{
    public EmployeeFaker() : base("sk")
    {
        RuleFor(e => e.EmployeeNumber, f => $"EMP-{f.Random.AlphaNumeric(8)}");
        RuleFor(e => e.Title, f => f.PickRandom<string?>(new[] { "Ing.", "Mgr.", "Bc.", null }));
        RuleFor(e => e.FirstName, f => f.Name.FirstName());
        RuleFor(e => e.LastName, f => f.Name.LastName());
        RuleFor(e => e.Email, (f, e) =>
            $"{e.FirstName}.{e.LastName}.{Guid.NewGuid():N}@example.com".ToLower());
        RuleFor(e => e.Phone, f => $"+4219{f.Random.ReplaceNumbers("########")}");
        RuleFor(e => e.CompanyId, 1);
    }
}