namespace REMS.Modules.Features.Agent;

public class DA_Agent
{
    private readonly AppDbContext _db;

    public DA_Agent(AppDbContext db)
    {
        _db = db;
    }

    public async Task<MessageResponseModel> CreateAgentAsync(AgentRequestModel requestModel)
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
            await _db.Agents.AddAsync(requestModel.ChangeAgent());
            int addAgent = await _db.SaveChangesAsync();
            var response = addAgent > 0
                ? new MessageResponseModel(true, "Successfully Save")
                : new MessageResponseModel(false, "Agent Register Fail");
            return response;
        }
        catch (Exception ex)
        {
            return new MessageResponseModel(false, ex);
        }
    }

    public async Task<MessageResponseModel> UpdateAgentAsync(int id, AgentRequestModel requestModel)
    {
        try
        {
            var user = await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == id);
            var agent = await _db.Agents
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == id);
            if (user is null || agent is null)
            {
                return new MessageResponseModel(false, "User Not Found");
            }

            if (!string.IsNullOrWhiteSpace(requestModel.AgentName))
            {
                user.Name = requestModel.AgentName;
                agent.AgencyName = requestModel.AgentName;
            }

            if (!string.IsNullOrWhiteSpace(requestModel.LicenseNumber))
            {
                agent.LicenseNumber = requestModel.LicenseNumber;
            }

            if (!string.IsNullOrWhiteSpace(requestModel.Email))
            {
                user.Email = requestModel.Email;
                agent.Email = requestModel.Email;
            }

            if (!string.IsNullOrWhiteSpace(requestModel.Password))
            {
                user.Password = requestModel.Password;
            }

            if (!string.IsNullOrWhiteSpace(requestModel.Phone))
            {
                user.Phone = requestModel.Phone;
                agent.Phone = requestModel.Phone;
            }

            if (!string.IsNullOrWhiteSpace(requestModel.Address))
            {
                agent.Address = requestModel.Address;
            }

            _db.Entry(user).State = EntityState.Modified;
            _db.Entry(agent).State = EntityState.Modified;
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

    public async Task<MessageResponseModel> DeleteAgentAsync(int userId)
    {
        try
        {
            var user = await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId);
            var agent = await _db.Agents
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if (user is null || agent is null)
            {
                return new MessageResponseModel(false, "User Not Found.");
            }

            _db.Users.Remove(user);
            _db.Entry(user).State = EntityState.Deleted;
            _db.Agents.Remove(agent);
            _db.Entry(agent).State = EntityState.Deleted;
            int result = await _db.SaveChangesAsync();
            var response = result > 0
                ? new MessageResponseModel(true, "Successfully Deleted.")
                : new MessageResponseModel(false, "Deleting Failed.");
            return response;
        }
        catch (Exception ex)
        {
            return new MessageResponseModel(false, ex);
        }
    }

    public async Task<MessageResponseModel> LoginAgentAsync(AgentLoginRequestModel agentLoginInfo)
    {
        var model = new MessageResponseModel();
        try
        {
            User? user = await _db.Users
                .Where(us => us.Name == agentLoginInfo.UserName && us.Password == agentLoginInfo.Password)
                .FirstOrDefaultAsync();

            // if (user != null)
            // {
            //     return new MessageResponseModel(true, "Login Success");
            // }
            // else
            // {
            //     return new MessageResponseModel(false, "Login Fail");
            // }

            if (user is null)
            {
                model = new MessageResponseModel(false, "Login Fail");
                goto result;
            }

            model = new MessageResponseModel(true, "Login Success");
        }
        catch (Exception ex)
        {
            model = new MessageResponseModel(true, ex.ToString());
        }

    result:
        return model;
    }

    public async Task<AgentResponseModel> SearchAgentAsync(int id)
    {
        AgentResponseModel model = new AgentResponseModel();
        try
        {
            AgentDto? agent = await _db.Agents
                .Where(ag => ag.AgentId == id)
                .Select(ag => new AgentDto
                {
                    AgentId = ag.AgentId,
                    UserId = ag.UserId,
                    AgencyName = ag.AgencyName,
                    LicenseNumber = ag.LicenseNumber,
                    Phone = ag.Phone,
                    Email = ag.Email,
                    Address = ag.Address
                })
                .FirstOrDefaultAsync();

            if (agent is null)
            {
                model.Status = "User Not Found";
            }
            else
            {
                model.IsSuccess = true;
                model.Status = "Success";
            }

            model.Agent = agent;
            return model;
        }
        catch (Exception ex)
        {
            model.Status = ex.ToString();
            return model;
        }
    }

    public async Task<AgentListResponseModel> SearchAgentByNameAsync(string? name, int pageNumber, int pageSize)
    {
        AgentListResponseModel model = new AgentListResponseModel();
        try
        {
            List<AgentDto> agents = await _db.Agents
                .Where(ag => string.IsNullOrEmpty(name) || ag.AgencyName.Contains(name))
                .OrderBy(ag => ag.AgencyName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(ag => new AgentDto
                {
                    AgentId = ag.AgentId,
                    UserId = ag.UserId,
                    AgencyName = ag.AgencyName,
                    LicenseNumber = ag.LicenseNumber,
                    Phone = ag.Phone,
                    Email = ag.Email,
                    Address = ag.Address
                })
                .ToListAsync();
            int rowCount = _db.Agents.Count();
            int pageCount = rowCount / pageSize;
            if (pageCount % pageSize > 0)
                pageCount++;
            model.IsSuccess = true;
            model.Status = "Success";
            model.AgentList = agents;
            model.PageCount = pageCount;
            model.PageNumber = pageNumber;
            model.PageSize = pageSize;
            return model;
        }
        catch (Exception ex)
        {
            model.Status = ex.ToString();
            return model;
        }
    }

    public async Task<Result<AgentListResponseModel>> SearchAgentByNameAsyncV2(string? name, int pageNumber, int pageSize)
    {
        Result<AgentListResponseModel> model = null;
        try
        {
            List<AgentDto> agents = await _db.Agents
                .Where(ag => string.IsNullOrEmpty(name) || ag.AgencyName.Contains(name))
                .OrderBy(ag => ag.AgencyName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(ag => new AgentDto
                {
                    AgentId = ag.AgentId,
                    UserId = ag.UserId,
                    AgencyName = ag.AgencyName,
                    LicenseNumber = ag.LicenseNumber,
                    Phone = ag.Phone,
                    Email = ag.Email,
                    Address = ag.Address
                })
                .ToListAsync();
            int rowCount = await _db.Agents.CountAsync();
            int pageCount = rowCount / pageSize;
            if (pageCount % pageSize > 0)
                pageCount++;
            var data = new AgentListResponseModel
            {
                AgentList = agents,
                PageCount = pageCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            model = Result<AgentListResponseModel>.Success(data);
        }
        catch (Exception ex)
        {
            model = Result<AgentListResponseModel>.Error(ex);
        }
        return model;
    }

    public async Task<AgentListResponseModel> SearchAgentByNameAndLocationAsync(string name, string location, int pageNumber, int pageSize)
    {
        AgentListResponseModel model = new AgentListResponseModel();
        try
        {
            List<AgentDto> agents = await _db.Agents
                .Where(ag => ag.AgencyName.Contains(name) && ag.Address != null && ag.Address.Contains(location))
                .OrderBy(ag => ag.AgencyName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(ag => new AgentDto
                {
                    AgentId = ag.AgentId,
                    UserId = ag.UserId,
                    AgencyName = ag.AgencyName,
                    LicenseNumber = ag.LicenseNumber,
                    Phone = ag.Phone,
                    Email = ag.Email,
                    Address = ag.Address
                })
                .ToListAsync();
            int rowCount = _db.Agents.Count();
            int pageCount = rowCount / pageSize;
            if (pageCount % pageSize > 0)
                pageCount++;
            model.IsSuccess = true;
            model.Status = "Success";
            model.AgentList = agents;
            model.PageCount = pageCount;
            model.PageNumber = pageNumber;
            model.PageSize = pageSize;
            return model;
        }
        catch (Exception ex)
        {
            model.Status = ex.ToString();
            return model;
        }
    }
}