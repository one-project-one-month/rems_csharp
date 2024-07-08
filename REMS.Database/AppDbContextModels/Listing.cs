using System;
using System.Collections.Generic;

namespace REMS.Database.AppDbContextModels;

public partial class Listing
{
    public int ListingId { get; set; }

    public int? PropertyId { get; set; }

    public int? AgentId { get; set; }

    public DateTime? DateListed { get; set; }

    public decimal ListingPrice { get; set; }

    public string Status { get; set; } = null!;

    public string? Description { get; set; }

    public virtual Agent? Agent { get; set; }

    public virtual Property? Property { get; set; }
}
