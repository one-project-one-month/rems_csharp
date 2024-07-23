namespace REMS.Models.Client;

public class ClientRequestModel
{
    public int? UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string Password { get; set; }

    public string? Address { get; set; }

    public DateTime DateCreate { get; set; } = DateTime.Now;
}
