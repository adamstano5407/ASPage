// Company.csx
public class Company
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Code { get; set; } = "";
    public int? ManagerId { get; set; }

    public override string ToString() =>
        $"Id: {Id}, Name: {Name}, Code: {Code}, ManagerId: {ManagerId}";
}