using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Models.Agent;

public class AgentDto
{
    public int AgentId { get; set; }

    public int? UserId { get; set; }

    public string? AgencyName { get; set; } = null!;

    public string? LicenseNumber { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }
}