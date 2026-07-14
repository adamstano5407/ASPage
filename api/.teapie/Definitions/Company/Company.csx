// Company.csx

#load ../IClone.csx
public class Company : IClone<Company> 
{
    public int? Id { get; set; }
    public string Name { get; set; } = "";
    public string Code { get; set; } = "";
    public int? ManagerId { get; set; }

    public override string ToString() =>
        $"Id: {Id}, Name: {Name}, Code: {Code}, ManagerId: {ManagerId}";

    public Company Clone()
    {
        return new Company
        {
            Name = Name,
            Code = Code,
            ManagerId = ManagerId
        };
    }
}