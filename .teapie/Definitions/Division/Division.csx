// Division.csx

#load ../IClone.csx

public class Division : IClone<Division>
{
    public int? Id { get; set; }
    public string Name { get; set; } = "";
    public string Code { get; set; } = "";
    public int CompanyId { get; set; }
    public int? ManagerId { get; set; }

    public override string ToString() =>
        $"Id: {Id}, Name: {Name}, Code: {Code}, CompanyId: {CompanyId}, ManagerId: {ManagerId}";

    public Division Clone()
    {
        return new Division
        {
            Name = Name,
            Code = Code,
            CompanyId = CompanyId,
            ManagerId = ManagerId
        };
    }
}