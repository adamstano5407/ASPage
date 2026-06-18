

using System.ComponentModel.DataAnnotations;

namespace APIKros.Models
{
    public class Company : HierarchyNode
    {

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();

        public ICollection<Division> Divisions { get; set; } = new List<Division>();
    }

}