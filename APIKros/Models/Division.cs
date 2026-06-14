namespace APIKros.Models { 
public class Division
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;

        public int CompanyId { get; set; }

        public Company Company { get; set; } = null!;

        public int ManagerId { get; set; } 

        public Employee Manager { get; set; } = null!;

        

    
    }
}