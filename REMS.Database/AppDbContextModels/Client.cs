using System;
using System.Collections.Generic;

namespace REMS.Database.AppDbContextModels;

public partial class Client
{
    public int ClientId { get; set; }

    public int? UserId { get; set; }

    public int? AgentId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public virtual Agent? Agent { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Transaction> TransactionBuyers { get; set; } = new List<Transaction>();

    public virtual ICollection<Transaction> TransactionSellers { get; set; } = new List<Transaction>();

    public virtual User? User { get; set; }
}
