using System;
using System.Collections.Generic;

namespace REMS.Database.AppDbContextModels;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int? PropertyId { get; set; }

    public int? BuyerId { get; set; }

    public int? SellerId { get; set; }

    public int? AgentId { get; set; }

    public DateTime? TransactionDate { get; set; }

    public decimal SalePrice { get; set; }

    public decimal? Commission { get; set; }

    public string Status { get; set; } = null!;

    public virtual Agent? Agent { get; set; }

    public virtual Client? Buyer { get; set; }

    public virtual Property? Property { get; set; }

    public virtual Client? Seller { get; set; }
}
