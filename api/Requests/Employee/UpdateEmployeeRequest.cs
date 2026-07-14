namespace APIKros.Requests.Employee
{
    public class UpdateEmployeeRequest
    {
        public int Id { get; set; } 
        public string? Title { get; set; }
        public string? EmployeeNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int? CompanyId { get; set; }
    }
}
