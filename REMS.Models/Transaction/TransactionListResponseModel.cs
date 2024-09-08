using REMS.Models.Custom;

namespace REMS.Models.Transaction;

public class TransactionListResponseModel
{
    public PageSettingModel? pageSetting { get; set; }

    public List<TransactionResponseModel>? lstTransaction { get; set; }
}