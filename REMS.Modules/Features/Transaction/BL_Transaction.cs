//using REMS.Modules.Features.TransactionModel;

namespace REMS.Modules.Features.Transaction;

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

    public async Task<Result<TransactionListResponseModel>> GetTransactionsByPropertyIdAsync(int pageNumber,
        int pageSize, int propertyId)
    {
        return await _daTransaction.GetTransactionsByPropertyIdAsync(pageNumber, pageSize, propertyId);
    }

    public async Task<Result<TransactionListResponseModel>> GetTransactionsByPropertyIdAndClientIdAsync(int propertyId,
        int buyerId, int pageNo, int pageSize)
    {
        return await _daTransaction.GetTransactionsByPropertyIdAndClientIdAsync(propertyId, buyerId, pageNo, pageSize);
    }

    public async Task<Result<TransactionListResponseModel>> GetTransactionsByClientIdAsync(int clientId, int pageNo,
        int pageSize)
    {
        return await _daTransaction.GetTransactionsByClientIdAsync(clientId, pageNo, pageSize);
    }
}