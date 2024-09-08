namespace REMS.Database.AppDbContextModels;

public class Transaction
{
    public int TransactionId { get; set; }

    public int? PropertyId { get; set; }

    public int? ClientId { get; set; }

    public DateTime? TransactionDate { get; set; }

    public int? RentalPeriod { get; set; }

    public decimal SalePrice { get; set; }

    public decimal? Commission { get; set; }

    public string Status { get; set; } = null!;

    public virtual Client? Client { get; set; }

    public virtual Property? Property { get; set; }
}