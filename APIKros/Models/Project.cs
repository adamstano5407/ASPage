

using System.ComponentModel.DataAnnotations;

namespace APIKros.Models
{
    public class Project
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Code { get; set; } = null!;

    public int DivisionId { get; set; }
    public Division Division { get; set; } = null!;

    public int? ManagerId { get; set; }
    public Employee? Manager { get; set; }

    public ICollection<Department> Departments { get; set; } = new List<Department>();
}
}

