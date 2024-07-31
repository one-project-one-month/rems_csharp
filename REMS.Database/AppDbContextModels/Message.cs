using System;
using System.Collections.Generic;

namespace REMS.Database.AppDbContextModels;

public partial class Message
{
    public int MessageId { get; set; }

    public int? SenderId { get; set; }

    public int? ReceiverId { get; set; }

    public int? PropertyId { get; set; }

    public string MessageContent { get; set; } = null!;

    public DateTime? DateSent { get; set; }

    public string? Status { get; set; }

    public virtual Property? Property { get; set; }

    public virtual User? Receiver { get; set; }

    public virtual User? Sender { get; set; }
}
