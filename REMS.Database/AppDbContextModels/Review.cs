using System;
using System.Collections.Generic;

namespace REMS.Database.AppDbContextModels;

public partial class Review
{
    public int ReviewId { get; set; }

    public int? UserId { get; set; }

    public int? PropertyId { get; set; }

    public int Rating { get; set; }

    public string? Comments { get; set; }

    public DateTime? DateCreated { get; set; }

    public virtual Property? Property { get; set; }

    public virtual User? User { get; set; }
}
