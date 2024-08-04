using Microsoft.EntityFrameworkCore;
using REMS.Models;
using System.Data;

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
            //var clients = await _db
            //    .Clients
            //    .AsNoTracking()
            //    .ToListAsync();

            var clients = await _db
            .Clients
            .Include(c => c.User)
            .AsNoTracking()
            .ToListAsync();

            var clientResponseModel = clients.Select(client => new ClientResponseModel
            {
                Client = client.Change(client.User)
            }).ToList();

            var clientListResponse = new ClientListResponseModel
            {
                DataLst = clientResponseModel,
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

    public async Task<Result<ClientListResponseModel>> GetClients(int pageNo = 1, int pageSize = 10)
    {
        Result<ClientListResponseModel> model = null;
        var responseModel = new ClientListResponseModel();
        try
        {
            var totalCount = await _db.Clients.CountAsync();
            var pageCount = (int)Math.Ceiling((double)totalCount / pageSize);

            var clients = await _db.Clients
                .AsNoTracking()
                .Include(c => c.User)
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var clientResponseModel = clients.Select(client => new ClientResponseModel
            {
                Client = client.Change(client.User!)
            }).ToList();

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

    public async Task<Result<ClientResponseModel>> GetClientById(int id)
    {
        Result<ClientResponseModel> model = null;
        try
        {
            var client = await _db
                .Clients
                .Include(c => c.User) // Include the User entity
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ClientId == id);

            if (client is null)
            {
                throw new Exception("Client Not Found");
            }

            var responseModel = new ClientResponseModel
            {
                Client = client.Change(client.User)
            };

            model = Result<ClientResponseModel>.Success(responseModel);
        }
        catch (Exception ex)
        {
            model = Result<ClientResponseModel>.Error(ex);
        }
        return model;
    }

    public async Task<Result<ClientResponseModel>> CreateClient(ClientRequestModel requestModel)
    {
        Result<ClientResponseModel> model = null;
        try
        {
            if (requestModel == null)
            {
                throw new ArgumentNullException(nameof(requestModel), "Request model cannot be null");
            }

            if (CheckEmailDuplicate(requestModel.Email))
            {
                model = Result<ClientResponseModel>.Error("Client create failed. Email already exist");
                goto result;
            }

            await _db.Users.AddAsync(requestModel.ChangeUser());
            int result = await _db.SaveChangesAsync();
            if (result < 0)
            {
                model = Result<ClientResponseModel>.Error("Client create failed.");
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
            int addClient = await _db.SaveChangesAsync();

            var responseModel = new ClientResponseModel
            {
                Client = client.Change(user),
            };

            model = addClient > 0
                ? Result<ClientResponseModel>.Success(responseModel)
                : Result<ClientResponseModel>.Error("Client create failed.");
        }
        catch (Exception ex)
        {
            model = Result<ClientResponseModel>.Error(ex);
        }
    result:
        return model;
    }

    public async Task<Result<ClientResponseModel>> UpdateClient(int id, ClientRequestModel requestModel)
    {
        Result<ClientResponseModel> model = null;
        try
        {
            var client = await _db.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ClientId == id);

            if (client is null)
            {
                return model = Result<ClientResponseModel>.Error("Client Not Found");
                goto result;
            }

            var user = await _db.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.UserId == client.UserId);

            if (user is null)
            {
                return model = Result<ClientResponseModel>.Error("User Not Found");
                goto result;
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
            }

            if (!string.IsNullOrWhiteSpace(requestModel.Email))
            {
                user.Email = requestModel.Email;
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

            var clientResponseModel = new ClientResponseModel
            {
                Client = client.Change(user)
            };

            model = Result<ClientResponseModel>.Success(clientResponseModel);
        }
        catch (Exception ex)
        {
            model = Result<ClientResponseModel>.Error(ex);
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

            if (client is null)
            {
                return model = Result<object>.Error("Client Not Found");
            }

            var user = await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == client.UserId);

            if (user is null || client is null)
            {
                return model = Result<object>.Error("User Not Found");
            }

            _db.Users.Remove(user);
            _db.Entry(user).State = EntityState.Deleted;
            _db.Clients.Remove(client);
            _db.Entry(client).State = EntityState.Deleted;
            int result = await _db.SaveChangesAsync();

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