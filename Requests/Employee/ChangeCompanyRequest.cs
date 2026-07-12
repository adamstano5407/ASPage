

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APIKros.Requests.Employee
{
    public class ChangeCompanyRequest
    {
        [JsonIgnore]
        public int EmployeeId { get; set; }
        
        [Required]
        public int NewCompanyId { get; set; }
    
    }

    
}