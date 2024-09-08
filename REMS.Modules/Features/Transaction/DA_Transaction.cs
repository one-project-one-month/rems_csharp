namespace REMS.Modules.Features.Transaction;

public class DA_Transaction
{
    private readonly AppDbContext _db;

    public DA_Transaction(AppDbContext db)
    {
        _db = db;
    }

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
                .Where(x => x.ClientId == transactionRequestModel.ClientId)
                .FirstOrDefaultAsync();

            if (buyerClient is null)
            {
                model = Result<string>.Error("Client Not Found!");
                goto result;
            }

            await _db.Transactions.AddAsync(transactionRequestModel.Change());
            var result = await _db.SaveChangesAsync();
            if (result > 0)
            {
                if (property.AvailiablityType.Equals(PropertyAvailiableType.Sell))
                    property.Status = PropertyStatus.Sold.ToString();
                else
                    property.Status = PropertyStatus.Rented.ToString();
                _db.Properties.Update(property);
                var propertyResult = await _db.SaveChangesAsync();
            }

            model = result > 0
                ? Result<string>.Success("Transaction creation success.")
                : Result<string>.Error("Transaction creation fail.");
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
            var transactionListResponse = new TransactionListResponseModel();
            var transactionList = await _db.Transactions.AsNoTracking()
                .Include(x => x.Client)
                .Include(x => x.Property)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var rowCount = _db.Transactions.Count();
            var pageCount = rowCount / pageSize;
            if (pageCount % pageSize > 0)
                pageCount++;

            transactionListResponse.pageSetting = new PageSettingModel(pageNumber, pageSize, pageCount, rowCount);

            var propertyResponseModel = transactionList.Select(Transactions => new TransactionResponseModel
            {
                Transaction = Transactions.Change(),
                Client = Transactions.Client.Change(_db.Users.Where(x => x.UserId == Transactions.Client.UserId)
                    .FirstOrDefault()),
                Property = Transactions.Property.Change()
            }).ToList();
            transactionListResponse.lstTransaction = propertyResponseModel;
            model = Result<TransactionListResponseModel>.Success(transactionListResponse);
            return model;
        }
        catch (Exception ex)
        {
            return model = Result<TransactionListResponseModel>.Error(ex);
        }
    }

    public async Task<Result<TransactionListResponseModel>> GetTransactionsByPropertyIdAsync(int PropertyId, int pageNo,
        int pageSize)
    {
        Result<TransactionListResponseModel> model = null;
        try
        {
            var transactionListResponse = new TransactionListResponseModel();
            var Query = _db.Transactions.AsQueryable();

            if (PropertyId != 0)
                Query = Query.Where(x => x.PropertyId == PropertyId);

            var transactionList = await Query.Include(x => x.Client)
                .Include(x => x.Property)
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (transactionList is null)
            {
                model = Result<TransactionListResponseModel>.Error("This property doesn't have in the transaction!");
                goto result;
            }

            var rowCount = _db.Transactions.Count();
            var pageCount = rowCount / pageSize;
            if (pageCount % pageSize > 0)
                pageCount++;

            transactionListResponse.pageSetting = new PageSettingModel(pageNo, pageSize, pageCount, rowCount);

            var transactionresponseModel = transactionList.Select(Transactions => new TransactionResponseModel
            {
                Transaction = Transactions.Change(),
                Client = Transactions.Client.Change(
                    _db.Users.FirstOrDefault(x => x.UserId == Transactions.Client.UserId)),
                Property = Transactions.Property.Change()
            }).ToList();
            transactionListResponse.lstTransaction = transactionresponseModel;
            model = Result<TransactionListResponseModel>.Success(transactionListResponse);
            return model;
        }
        catch (Exception ex)
        {
            return model = Result<TransactionListResponseModel>.Error(ex);
        }

    result:
        return model;
    }

    public async Task<Result<TransactionListResponseModel>> GetTransactionsByPropertyIdAndClientIdAsync(int propertyId,
        int clientId, int pageNo, int pageSize)
    {
        Result<TransactionListResponseModel> model = null;
        try
        {
            var Query = _db.Transactions.AsQueryable();
            if (propertyId != 0)
                Query = Query.Where(x => x.PropertyId == propertyId);
            if (clientId != 0)
                Query = Query.Where(x => x.ClientId == clientId);

            var transactionListResponse = new TransactionListResponseModel();

            var transactionList = await Query.Include(x => x.Client)
                .Include(x => x.Property)
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            if (transactionList is null)
            {
                model = Result<TransactionListResponseModel>.Error("Property Not Found!");
                goto result;
            }

            var rowCount = _db.Transactions.Count();
            var pageCount = rowCount / pageSize;
            if (pageCount % pageSize > 0)
                pageCount++;

            transactionListResponse.pageSetting = new PageSettingModel(pageNo, pageSize, pageCount, rowCount);
            var transactionresponseModel = transactionList.Select(Transactions => new TransactionResponseModel
            {
                Transaction = Transactions.Change(),
                Client = Transactions.Client.Change(
                    _db.Users.FirstOrDefault(x => x.UserId == Transactions.Client.UserId)),
                Property = Transactions.Property.Change()
            }).ToList();
            transactionListResponse.lstTransaction = transactionresponseModel;
            model = Result<TransactionListResponseModel>.Success(transactionListResponse);
            return model;
        }
        catch (Exception ex)
        {
            return model = Result<TransactionListResponseModel>.Error(ex);
        }

    result:
        return model;
    }

    public async Task<Result<TransactionListResponseModel>> GetTransactionsByClientIdAsync(int clientId, int pageNo,
        int pageSize)
    {
        Result<TransactionListResponseModel> model = null;
        try
        {
            var transactionListResponse = new TransactionListResponseModel();

            var Query = _db.Transactions.AsQueryable();

            if (clientId != 0)
                Query = Query.Where(x => x.ClientId == clientId);

            var transactionList = await Query.Include(x => x.Client)
                .Include(x => x.Property)
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (transactionList is null)
            {
                model = Result<TransactionListResponseModel>.Error("This client doesn't have in the transaction!!");
                goto result;
            }

            var rowCount = _db.Transactions.Count();
            var pageCount = rowCount / pageSize;
            if (pageCount % pageSize > 0)
                pageCount++;

            transactionListResponse.pageSetting = new PageSettingModel(pageNo, pageSize, pageCount, rowCount);
            var transactionresponseModel = transactionList.Select(Transactions => new TransactionResponseModel
            {
                Transaction = Transactions.Change(),
                Client = Transactions.Client.Change(
                    _db.Users.FirstOrDefault(x => x.UserId == Transactions.Client.UserId)),
                Property = Transactions.Property.Change()
            }).ToList();
            transactionListResponse.lstTransaction = transactionresponseModel;
            model = Result<TransactionListResponseModel>.Success(transactionListResponse);
            return model;
        }
        catch (Exception ex)
        {
            return model = Result<TransactionListResponseModel>.Error(ex);
        }

    result:
        return model;
    }
}