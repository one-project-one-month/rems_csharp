namespace REMS.Database.AppDbContextModels;

public class Agent
{
    public int AgentId { get; set; }

    public int? UserId { get; set; }

    public string AgencyName { get; set; } = null!;

    public string LicenseNumber { get; set; } = null!;

    public string? Address { get; set; }

    public virtual ICollection<Property> Properties { get; set; } = new List<Property>();

    public virtual User? User { get; set; }
}