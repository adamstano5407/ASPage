

using System.ComponentModel.DataAnnotations;

namespace APIKros.Models
{
    public class Company(string name, string code, int? managerId) : HierarchyNode(name, code, managerId)
    {
        
        private readonly List<Employee> _employees = new();
        public IReadOnlyCollection<Employee> Employees => _employees;

        private readonly List<Division> _divisions = new();
        public IReadOnlyCollection<Division> Divisions => _divisions;

        public void AddEmployee(Employee employee)
        {
            _employees.Add(employee);
        }

        public void AddDivision(Division division)
        {
            _divisions.Add(division);
        }
    }

}