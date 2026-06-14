

namespace APIKros.Models
{

    public class Company
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Code { get; set; } = null!;


        public int? DirectorId { get; set; }
        public Employee? Director { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();

        public ICollection<Division> Divisions { get; set; } = new List<Division>();
    }

}