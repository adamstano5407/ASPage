// DepartmentFaker.csx
#nuget "Bogus, 35.6.5"

#load "Department.csx"

public class DepartmentFaker : Bogus.Faker<Department>
{
    public DepartmentFaker() : base("sk")
    {
        RuleFor(d => d.Name, f => f.Commerce.Department());
        RuleFor(d => d.Code, f => f.Random.AlphaNumeric(5).ToUpper());
        RuleFor(d => d.ProjectId, 1);
        RuleFor(d => d.ManagerId, 1);
    }
}