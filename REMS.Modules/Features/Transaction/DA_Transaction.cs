using REMS.Models.Appointment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Modules.Features.Transaction
{
    public class DA_Transaction
    {
        private readonly AppDbContext _db;

        public DA_Transaction(AppDbContext db) => _db = db;

        public async Task<Result<string>> CreateTransactionAsync(TransactionRequestModel transactionRequestModel)
        {
            Result<string> model = null;
            try
            {
                var property = await _db.Properties
                    .Where(x => x.PropertyId == transactionRequestModel.PropertyId)
                    .FirstOrDefaultAsync();

                if (property is null)
                {
                    model = Result<string>.Error("Property Not Found!");
                    goto result;
                }

                var buyerClient = await _db.Clients
                    .Where(x => x.ClientId == transactionRequestModel.BuyerId)
                    .FirstOrDefaultAsync();

                if (buyerClient is null)
                {
                    model = Result<string>.Error("Buyer Not Found!");
                    goto result;
                }

                var sellerClient = await _db.Clients
                    .Where(x => x.ClientId == transactionRequestModel.SellerId)
                    .FirstOrDefaultAsync();

                if (sellerClient is null)
                {
                    model = Result<string>.Error("Seller Not Found!");
                    goto result;
                }

                var Agent = await _db.Agents
                   .Where(x => x.AgentId == transactionRequestModel.AgentId)
                   .FirstOrDefaultAsync();

                if (Agent is null)
                {
                    model = Result<string>.Error("Agent Not Found!");
                    goto result;
                }
                await _db.Transactions.AddAsync(transactionRequestModel.Change());
                int result = await _db.SaveChangesAsync();
                model = result > 0 ? Result<string>.Success("Transaction creation success.") : Result<string>.Error("Transaction creation fail.");
            }
            catch (Exception ex)
            {
                model = Result<string>.Error(ex);
            }

        result:
            return model;
        }

        public async Task<Result<TransactionListResponseModel>> GetTransactionsAsync(int pageNumber, int pageSize)
        {
            Result<TransactionListResponseModel> model = null;
            try
            {
                TransactionListResponseModel transactionListResponse = new TransactionListResponseModel();
                var transactionList = _db.Transactions.AsNoTracking();
                var transaction = await transactionList.Pagination(pageNumber, pageSize).Select(x => x.Change()).ToListAsync();

                int rowCount = _db.Transactions.Count();
                int pageCount = rowCount / pageSize;
                if (pageCount % pageSize > 0)
                    pageCount++;

                transactionListResponse.pageSetting = new PageSettingModel(pageNumber, pageSize, pageCount, rowCount);
                transactionListResponse.lstTransaction = transaction;
                model = Result<TransactionListResponseModel>.Success(transactionListResponse);
                return model;
            }
            catch (Exception ex)
            {
                return model = Result<TransactionListResponseModel>.Error(ex);
            }
        }

        public async Task<Result<TransactionListResponseModel>> GetTransactionsByPropertyIdAsync(int pageNumber, int pageSize, int PropertyId)
        {
            Result<TransactionListResponseModel> model = null;
            try
            {
                TransactionListResponseModel transactionListResponse = new TransactionListResponseModel();
                var transactionList = _db.Transactions.AsNoTracking().Where(x => x.PropertyId == PropertyId);
                var transaction = await transactionList.Pagination(pageNumber, pageSize).Select(x => x.Change()).ToListAsync();

                int rowCount = _db.Transactions.Count();
                int pageCount = rowCount / pageSize;
                if (pageCount % pageSize > 0)
                    pageCount++;

                transactionListResponse.pageSetting = new PageSettingModel(pageNumber, pageSize, pageCount, rowCount);
                transactionListResponse.lstTransaction = transaction;
                model = Result<TransactionListResponseModel>.Success(transactionListResponse);
                return model;
            }
            catch (Exception ex)
            {
                return model = Result<TransactionListResponseModel>.Error(ex);
            }
        }

        public async Task<Result<TransactionListResponseModel>> GetTransactionsByPropertyIdAndAgentIdAsync(int pageNumber, int pageSize, int propertyId, int agentId)
        {
            Result<TransactionListResponseModel> model = null;
            try
            {
                TransactionListResponseModel transactionListResponse = new TransactionListResponseModel();
                var transactionList = _db.Transactions.AsNoTracking().Where(x => x.PropertyId == propertyId);
                var transaction = await transactionList.Pagination(pageNumber, pageSize).Select(x => x.Change()).ToListAsync();

                int rowCount = _db.Transactions.Count();
                int pageCount = rowCount / pageSize;
                if (pageCount % pageSize > 0)
                    pageCount++;

                transactionListResponse.pageSetting = new PageSettingModel(pageNumber, pageSize, pageCount, rowCount);
                transactionListResponse.lstTransaction = transaction;
                model = Result<TransactionListResponseModel>.Success(transactionListResponse);
                return model;
            }
            catch (Exception ex)
            {
                return model = Result<TransactionListResponseModel>.Error(ex);
            }
        }

        public async Task<Result<TransactionListResponseModel>> GetTransactionsByPropertyIdAndBuyerIdAsync(int pageNumber, int pageSize, int propertyId, int buyerId)
        {
            Result<TransactionListResponseModel> model = null;
            try
            {
                TransactionListResponseModel transactionListResponse = new TransactionListResponseModel();
                var transactionList = _db.Transactions.AsNoTracking().Where(x => x.PropertyId == propertyId);
                var transaction = await transactionList.Pagination(pageNumber, pageSize).Select(x => x.Change()).ToListAsync();

                int rowCount = _db.Transactions.Count();
                int pageCount = rowCount / pageSize;
                if (pageCount % pageSize > 0)
                    pageCount++;

                transactionListResponse.pageSetting = new PageSettingModel(pageNumber, pageSize, pageCount, rowCount);
                transactionListResponse.lstTransaction = transaction;
                model = Result<TransactionListResponseModel>.Success(transactionListResponse);
                return model;
            }
            catch (Exception ex)
            {
                return model = Result<TransactionListResponseModel>.Error(ex);
            }
        }

        public async Task<Result<TransactionListResponseModel>> GetTransactionsByPropertyIdAndSellerIdAsync(int pageNumber, int pageSize, int propertyId, int sellerId)
        {
            Result<TransactionListResponseModel> model = null;
            try
            {
                TransactionListResponseModel transactionListResponse = new TransactionListResponseModel();
                var transactionList = _db.Transactions.AsNoTracking().Where(x => x.PropertyId == propertyId);
                var transaction = await transactionList.Pagination(pageNumber, pageSize).Select(x => x.Change()).ToListAsync();

                int rowCount = _db.Transactions.Count();
                int pageCount = rowCount / pageSize;
                if (pageCount % pageSize > 0)
                    pageCount++;

                transactionListResponse.pageSetting = new PageSettingModel(pageNumber, pageSize, pageCount, rowCount);
                transactionListResponse.lstTransaction = transaction;
                model = Result<TransactionListResponseModel>.Success(transactionListResponse);
                return model;
            }
            catch (Exception ex)
            {
                return model = Result<TransactionListResponseModel>.Error(ex);
            }
        }
    }
}
