using REMS.Models.Transaction;

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
                await _db.Transactions.AddAsync(transactionRequestModel.ChangeTransaction());
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
    }
}
