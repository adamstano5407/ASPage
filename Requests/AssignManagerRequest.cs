using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APIKros.Requests;

public class AssignManagerRequest
{
    [Required]
    public int EmployeeId { get; set; }

    [JsonIgnore]
    public int? NodeId { get; set; } = null;
}
