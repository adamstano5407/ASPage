using System.ComponentModel.DataAnnotations;

namespace APIKros.Models
{
    public class Department
    {
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Code { get; set; } = null!;
    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    public int? ManagerId { get; set; }
    public Employee? Manager { get; set; }
    }
}

