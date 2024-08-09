using System.Text.Json.Serialization;

namespace REMS.Models.Client;

public class ClientRequestModel
{
    [JsonIgnore] // Completely hides from API schema
    public int? UserId { get; set; }

    public string? FirstName { get; set; } = null!;

    public string? LastName { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Address { get; set; }

    [JsonIgnore]
    public DateTime DateCreate { get; set; } = DateTime.Now;
}
