// Department.csx

#load ../IClone.csx

public class Department : IClone<Department>
{
    public int? Id { get; set; }
    public string Name { get; set; } = "";
    public string Code { get; set; } = "";
    public int ProjectId { get; set; }
    public int? ManagerId { get; set; }

    public override string ToString() =>
        $"Id: {Id}, Name: {Name}, Code: {Code}, ProjectId: {ProjectId}, ManagerId: {ManagerId}";

    public Department Clone()
    {
        return new Department
        {
            Name = Name,
            Code = Code,
            ProjectId = ProjectId,
            ManagerId = ManagerId
        };
    }
}