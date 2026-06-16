namespace APIKros.Requests
{
    public class CreateEmployeeRequest
    {
        public string? Title { get; set; } = null;
        
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;

        public int CompanyId { get; set; }
    }

}