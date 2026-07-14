// Project.csx
#load ../IClone.csx

public class Project : IClone<Project>
{
    public int? Id { get; set; }
    public string Name { get; set; } = "";
    public string Code { get; set; } = "";
    public int DivisionId { get; set; }
    public int? ManagerId { get; set; }

    public override string ToString() =>
        $"Id: {Id}, Name: {Name}, Code: {Code}, DivisionId: {DivisionId}, ManagerId: {ManagerId}";

    public Project Clone()
    {
        return new Project
        {
            Name = Name,
            Code = Code,
            DivisionId = DivisionId,
            ManagerId = ManagerId
        };
    }
}