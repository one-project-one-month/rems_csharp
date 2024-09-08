namespace REMS.Modules.Features.Client;

public class DA_Client
{
    private readonly AppDbContext _db;

    public DA_Client(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Result<ClientListResponseModel>> GetClients()
    {
        Result<ClientListResponseModel> model = null;
        var responseModel = new ClientListResponseModel();
        try
        {
            var clients = await _db
                .Clients
                .Include(c => c.User)
                .AsNoTracking()
                .ToListAsync();

            var clientResponseModel = clients.Select(client => client.Change(client.User)).ToList();

            var clientListResponse = new ClientListResponseModel
            {
                DataLst = clientResponseModel
            };

            model = Result<ClientListResponseModel>.Success(clientListResponse);
        }
        catch (Exception ex)
        {
            model = Result<ClientListResponseModel>.Error(ex);
            return model;
        }

        return model;
    }

    public async Task<Result<ClientListResponseModel>> GetClients(string? firstName, string? lastName, string? email,
        string? phone, int pageNo = 1, int pageSize = 10)
    {
        Result<ClientListResponseModel> model = null;
        var responseModel = new ClientListResponseModel();
        try
        {
            var query = _db.Clients
                .Include(c => c.User) // Join User table
                .AsQueryable();

            #region Add filter

            // Apply filters based on provided parameters
            if (!string.IsNullOrEmpty(firstName)) query = query.Where(c => c.FirstName.Contains(firstName));

            if (!string.IsNullOrEmpty(lastName)) query = query.Where(c => c.LastName.Contains(lastName));

            if (!string.IsNullOrEmpty(email)) query = query.Where(c => c.User.Email.Contains(email));

            if (!string.IsNullOrEmpty(phone)) query = query.Where(c => c.User.Phone.Contains(phone));

            #endregion

            // Pagination logic
            var totalCount = await query.CountAsync();
            var pageCount = (int)Math.Ceiling((double)totalCount / pageSize);

            var clients = await query
                .AsNoTracking()
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var clientResponseModel = clients.Select(client => client.Change(client.User)).ToList();

            var clientListResponse = new ClientListResponseModel
            {
                DataLst = clientResponseModel,
                PageSetting = new PageSettingModel(pageNo, pageSize, pageCount, totalCount)
            };

            model = Result<ClientListResponseModel>.Success(clientListResponse);
        }
        catch (Exception ex)
        {
            model = Result<ClientListResponseModel>.Error(ex);
            return model;
        }

        return model;
    }

    public async Task<Result<ClientModel>> GetClientById(int id)
    {
        Result<ClientModel> model = null;
        try
        {
            var client = await _db
                .Clients
                .Include(c => c.User) // Include the User entity
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ClientId == id);

            if (client is null) return model = Result<ClientModel>.Error("Client not found.");

            var responseModel = client.Change(client.User);

            model = Result<ClientModel>.Success(responseModel);
        }
        catch (Exception ex)
        {
            model = Result<ClientModel>.Error(ex);
        }

        return model;
    }

    public async Task<Result<ClientModel>> CreateClient(ClientRequestModel requestModel)
    {
        Result<ClientModel> model = null;
        try
        {
            if (requestModel == null)
                throw new ArgumentNullException(nameof(requestModel), "Request model cannot be null");

            if (CheckEmailDuplicate(requestModel.Email))
            {
                model = Result<ClientModel>.Error("Client create failed. Email already exist");
                goto result;
            }

            await _db.Users.AddAsync(requestModel.ChangeUser());
            var result = await _db.SaveChangesAsync();
            if (result < 0)
            {
                model = Result<ClientModel>.Error("Client create failed.");
                goto result;
            }

            //To get UserId for client create
            var user = await _db.Users
                .OrderByDescending(x => x.UserId)
                .AsNoTracking()
                .FirstAsync();

            requestModel.UserId = user.UserId;

            var client = requestModel.Change();
            await _db.Clients.AddAsync(client);
            var addClient = await _db.SaveChangesAsync();

            var responseModel = client.Change(user);

            model = addClient > 0
                ? Result<ClientModel>.Success(responseModel)
                : Result<ClientModel>.Error("Client create failed.");
        }
        catch (Exception ex)
        {
            model = Result<ClientModel>.Error(ex);
        }

    result:
        return model;
    }

    public async Task<Result<ClientModel>> UpdateClient(int id, ClientRequestModel requestModel)
    {
        Result<ClientModel> model = null;
        try
        {
            var client = await _db.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ClientId == id);

            if (client is null)
            {
                return model = Result<ClientModel>.Error("Client Not Found");
                goto result;
            }

            var user = await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == client.UserId);

            if (user is null)
            {
                return model = Result<ClientModel>.Error("User Not Found");
                goto result;
            }

            if (!string.IsNullOrWhiteSpace(requestModel.FirstName) || !string.IsNullOrWhiteSpace(requestModel.LastName))
            {
                var firstName = requestModel.FirstName ?? string.Empty;
                var lastName = requestModel.LastName ?? string.Empty;
                var Name = string.Concat(firstName, !string.IsNullOrEmpty(firstName) ? " " : string.Empty, lastName);
                user.Name = Name;
                client.FirstName = requestModel.FirstName ?? client.FirstName;
                client.LastName = requestModel.LastName ?? client.LastName;
            }

            if (!string.IsNullOrWhiteSpace(requestModel.Phone)) user.Phone = requestModel.Phone;

            if (!string.IsNullOrWhiteSpace(requestModel.Email)) user.Email = requestModel.Email;

            if (!string.IsNullOrWhiteSpace(requestModel.Password)) user.Password = requestModel.Password;

            if (!string.IsNullOrWhiteSpace(requestModel.Address)) client.Address = requestModel.Address;

            _db.Entry(user).State = EntityState.Modified;
            _db.Entry(client).State = EntityState.Modified;
            var result = await _db.SaveChangesAsync();

            var clientResponseModel = client.Change(user);

            model = Result<ClientModel>.Success(clientResponseModel);
        }
        catch (Exception ex)
        {
            model = Result<ClientModel>.Error(ex);
        }

    result:
        return model;
    }

    public async Task<Result<object>> DeleteClient(int id)
    {
        Result<object> model = null;
        try
        {
            var client = await _db.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ClientId == id);

            if (client is null) return model = Result<object>.Error("Client Not Found");

            var user = await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == client.UserId);

            if (user is null || client is null) return model = Result<object>.Error("User Not Found");

            _db.Users.Remove(user);
            _db.Entry(user).State = EntityState.Deleted;
            _db.Clients.Remove(client);
            _db.Entry(client).State = EntityState.Deleted;
            var result = await _db.SaveChangesAsync();

            model = result > 0
                ? Result<object>.Success(null)
                : Result<object>.Error("Delete failed.");
        }
        catch (Exception ex)
        {
            return model = Result<object>.Error(ex);
        }

        return model;
    }

    private bool CheckEmailDuplicate(string email)
    {
        var isDuplicate = _db.Users.Any(x => x.Email == email);
        return isDuplicate;
    }
}