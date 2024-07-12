using Microsoft.EntityFrameworkCore;

namespace REMS.Modules.Features.Client;

public class DA_Client
{
    private readonly AppDbContext _db;

    public DA_Client(AppDbContext db)
    {
        _db = db;
    }

    public async Task<ClientListResponseModel> GetClients()
    {
        var responseModel = new ClientListResponseModel();
        try
        {
            var clients = await _db
                .Clients
                .AsNoTracking()
                .ToListAsync();
            responseModel.DataLst = clients
                .Select(x => x.Change())
                .ToList();
            responseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
        }
        catch (Exception ex)
        {
            responseModel.DataLst = new List<ClientResponseModel>();
            responseModel.MessageResponse = new MessageResponseModel(false, ex);
        }

        return responseModel;
    }

    public async Task<ClientListResponseModel> GetClients(int pageNo = 1, int pageSize = 10)
    {
        var responseModel = new ClientListResponseModel();
        try
        {
            var totalCount = await _db.Clients.CountAsync();
            var pageCount = (int)Math.Ceiling((double)totalCount / pageSize);

            var clients = await _db.Clients
                .AsNoTracking()
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            responseModel.DataLst = clients
                .Select(x => x.Change())
                .ToList();
            responseModel.MessageResponse = new MessageResponseModel(true, EnumStatus.Success.ToString());
            responseModel.PageSetting = new PageSettingModel(pageNo, pageSize, pageCount, totalCount);
        }
        catch (Exception ex)
        {
            responseModel.DataLst = new List<ClientResponseModel>();
            responseModel.MessageResponse = new MessageResponseModel(false, ex);
        }

        return responseModel;
    }

    public async Task<ClientResponseModel> GetClientById(int id)
    {
        var responseModel = new ClientResponseModel();
        try
        {
            var client = await _db
                .Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ClientId == id);
            if(client is null)
            {
                throw new Exception("Client Not Found");
            }
            
            responseModel = client.Change();
        }
        catch (Exception ex)
        {
            responseModel = new ClientResponseModel();
        }
        return responseModel;
    }

    public async Task<MessageResponseModel> CreateClientAsync(ClientRequestModel requestModel)
    {
        try
        {
            await _db.Users.AddAsync(requestModel.ChangeUser());
            int result = await _db.SaveChangesAsync();
            if (result < 0)
            {
                return new MessageResponseModel(false, "Registration Fail");
            }

            var user = await _db.Users
                .OrderByDescending(x => x.UserId)
                .AsNoTracking()
                .FirstAsync();
            requestModel.UserId = user.UserId;
            await _db.Clients.AddAsync(requestModel.Change());
            int addClient = await _db.SaveChangesAsync();
            var response = addClient > 0
                ? new MessageResponseModel(true, "Successfully Saved.")
                : new MessageResponseModel(false, "Client Registration Failed.");
            return response;
        }
        catch (Exception ex)
        {
            return new MessageResponseModel(false, ex);
        }
    }

    public async Task<MessageResponseModel> UpdateClientAsync(int id, ClientRequestModel requestModel)
    {
        try
        {
            //User user = new User();
            var client = await _db.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ClientId == id);

            if (client is null)
            {
                return new MessageResponseModel(false, "Client Not Found");
            }

            var user = await _db.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.UserId == client.UserId);

            if (user is null)
            {
                return new MessageResponseModel(false, "User Not Found");
            }

            if (!string.IsNullOrWhiteSpace(requestModel.FirstName) || !string.IsNullOrWhiteSpace(requestModel.LastName))
            {
                string firstName = requestModel.FirstName ?? string.Empty;
                string lastName = requestModel.LastName ?? string.Empty;
                string Name = string.Concat(firstName, (!string.IsNullOrEmpty(firstName) ? " " : string.Empty), lastName);
                user.Name = Name;
                client.FirstName = requestModel.FirstName ?? client.FirstName;
                client.LastName = requestModel.LastName ?? client.LastName;
            }

            if (!string.IsNullOrWhiteSpace(requestModel.Phone))
            {
                user.Phone = requestModel.Phone;
                client.Phone = requestModel.Phone;
            }

            if (!string.IsNullOrWhiteSpace(requestModel.Email))
            {
                user.Email = requestModel.Email;
                client.Email = requestModel.Email;
            }

            if (!string.IsNullOrWhiteSpace(requestModel.Password))
            {
                user.Password = requestModel.Password;
            }

            if (!string.IsNullOrWhiteSpace(requestModel.Address))
            {
                client.Address = requestModel.Address;
            }

            _db.Entry(user).State = EntityState.Modified;
            _db.Entry(client).State = EntityState.Modified;
            int result = await _db.SaveChangesAsync();
            var response = result > 0
                ? new MessageResponseModel(true, "Successfully Update")
                : new MessageResponseModel(false, "Updating Fail");
            return response;
        }
        catch (Exception ex)
        {
            return new MessageResponseModel(false, ex);
        }
    }
}