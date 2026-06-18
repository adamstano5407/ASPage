using System.ComponentModel.DataAnnotations;

namespace APIKros.Models;

public abstract class HierarchyNode
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Code { get; set; } = null!;


    public int? ManagerId { get; set; }
    public Employee? Manager { get; set; }
}