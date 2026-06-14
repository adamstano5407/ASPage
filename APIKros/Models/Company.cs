

using System.ComponentModel.DataAnnotations;

namespace APIKros.Models
{

    public class Company
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Code { get; set; } = null!;


        public int? DirectorId { get; set; }
        public Employee? Director { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();

        public ICollection<Division> Divisions { get; set; } = new List<Division>();
    }

}