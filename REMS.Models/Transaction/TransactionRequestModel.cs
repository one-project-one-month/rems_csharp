namespace REMS.Models.Transaction;

public class TransactionRequestModel
{
    public int TransactionId { get; set; }
    public int PropertyId { get; set; }
    public int ClientId { get; set; }
    public DateTime TransactionDate { get; set; }
    public decimal SalePrice { get; set; }
    public decimal? Commission { get; set; }
    public string Status { get; set; }
}