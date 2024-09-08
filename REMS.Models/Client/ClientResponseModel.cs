namespace REMS.Models.Client;

public class ClientResponseModel
{
    //public ClientModel Client { get; set; }
    public int ClientId { get; set; }

    public int? UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? Role { get; set; }
}