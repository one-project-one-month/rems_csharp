using System;
using System.Collections.Generic;

namespace REMS.Database.AppDbContextModels;

public partial class Agent
{
    public int AgentId { get; set; }

    public int? UserId { get; set; }

    public string AgencyName { get; set; } = null!;

    public string LicenseNumber { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Property> Properties { get; set; } = new List<Property>();

    public virtual User? User { get; set; }
}
