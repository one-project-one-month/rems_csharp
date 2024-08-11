//using REMS.Modules.Features.TransactionModel;

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

        public async Task<Result<TransactionListResponseModel>> GetTransactionsAsync(int pageNumber, int pageSize)
        {
            return await _daTransaction.GetTransactionsAsync(pageNumber, pageSize);
        }

        public async Task<Result<TransactionListResponseModel>> GetTransactionsByPropertyIdAsync(int pageNumber, int pageSize, int propertyId)
        {
            return await _daTransaction.GetTransactionsByPropertyIdAsync(pageNumber, pageSize, propertyId);
        }

        public async Task<Result<TransactionListResponseModel>> GetTransactionsByPropertyIdAndClientIdAsync(int pageNumber, int pageSize, int propertyId, int buyerId)
        {
            return await _daTransaction.GetTransactionsByPropertyIdAndClientIdAsync(pageNumber, pageSize, propertyId, buyerId);
        }

        public async Task<Result<TransactionListResponseModel>> GetTransactionsByClientIdAsync(int pageNumber, int pageSize, int clientId)
        {
            return await _daTransaction.GetTransactionsByClientIdAsync(pageNumber, pageSize, clientId);
        }
    }
}
