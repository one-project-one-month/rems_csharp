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

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

    public virtual ICollection<Listing> Listings { get; set; } = new List<Listing>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual User? User { get; set; }
}
