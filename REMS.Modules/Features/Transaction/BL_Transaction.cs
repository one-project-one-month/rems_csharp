using REMS.Models.Transaction;

namespace REMS.Modules.Features.Transaction
{
    public class BL_Transaction
    {
        private readonly DA_Transaction _daTransaction;

        public BL_Transaction(DA_Transaction daTransaction)
        {
            _daTransaction = daTransaction;
        }

        public async Task<Result<string>> CreateTransactionAsync(TransactionRequestModel transactionRequestModel)
        {
            return await _daTransaction.CreateTransactionAsync(transactionRequestModel);
        }
    }
}
