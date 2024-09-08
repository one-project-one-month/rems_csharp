namespace REMS.Models.Agent;

public class AgentDto
{
    public int AgentId { get; set; }

    public int? UserId { get; set; }

    public string? AgentName { get; set; }

    public string? AgencyName { get; set; } = null!;

    public string? LicenseNumber { get; set; } = null!;

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string? Role { get; set; }
}