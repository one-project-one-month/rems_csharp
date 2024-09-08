using REMS.Models.Client;
using REMS.Models.Property;

namespace REMS.Models.Transaction;

public class TransactionResponseModel
{
    public TransactionModel Transaction { get; set; }
    public ClientModel Client { get; set; }
    public PropertyModel Property { get; set; }
}