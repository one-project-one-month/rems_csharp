
namespace REMS.Modules.Features.Agent;

public class DA_Agent
{
    private readonly AppDbContext _db;

    public DA_Agent(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Result<string>> CreateAgentAsync(AgentRequestModel requestModel)
    {
        Result<string> response = null;
        try
        {
            await _db.Users.AddAsync(requestModel.ChangeUser());
            int result = await _db.SaveChangesAsync();
            if (result < 0)
            {
                return Result<string>.Error("Registration Fail");
            }

            var user = await _db.Users
                .OrderByDescending(x => x.UserId)
                .AsNoTracking()
                .FirstAsync();
            requestModel.UserId = user.UserId;
            await _db.Agents.AddAsync(requestModel.ChangeAgent());
            int addAgent = await _db.SaveChangesAsync();
            response = addAgent > 0
                ? Result<string>.Success("Agent Register Successfully")
                : Result<string>.Error("Agent Register Fail");
        }
        catch (Exception ex)
        {
            response = Result<string>.Error(ex);
        }
        return response;
    }

    public async Task<Result<string>> UpdateAgentAsync(int id, AgentRequestModel requestModel)
    {
        Result<string> response = null;
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
                return Result<string>.Error("User Not Found");
            }

            if (!string.IsNullOrWhiteSpace(requestModel.UserName))
            {
                user.Name = requestModel.UserName;
            }

            if (!string.IsNullOrWhiteSpace(requestModel.FirstName) && !string.IsNullOrWhiteSpace(requestModel.LastName))
            {
                agent.AgencyName = $"{requestModel.FirstName} {requestModel.LastName}";
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
            response = result > 0
                ? Result<string>.Success("Agent Update Successfully.")
                : Result<string>.Error("Updating Fail");

        }
        catch (Exception ex)
        {
            response = Result<string>.Error(ex);
        }
        return response;
    }

    public async Task<Result<string>> DeleteAgentAsync(int userId)
    {
        Result<string> response = null;
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
                return Result<string>.Error("User Not Found");
            }

            _db.Users.Remove(user);
            _db.Entry(user).State = EntityState.Deleted;
            _db.Agents.Remove(agent);
            _db.Entry(agent).State = EntityState.Deleted;
            int result = await _db.SaveChangesAsync();
            response = result > 0
                ? Result<string>.Success("Agent Delete Successfully.")
                : Result<string>.Error("Deleting Fail");
        }
        catch (Exception ex)
        {
            response = Result<string>.Error(ex);
        }
        return response;
    }

    public async Task<Result<string>> LoginAgentAsync(AgentLoginRequestModel agentLoginInfo)
    {
        Result<string> model = null;
        try
        {
            User? user = await _db.Users
                .Where(us => us.Name == agentLoginInfo.UserName && us.Password == agentLoginInfo.Password)
                .FirstOrDefaultAsync();

            if (user is null)
            {
                model = Result<string>.Error("Login Fail");
                goto result;
            }

            model = model = Result<string>.Error("Login Success");
        }
        catch (Exception ex)
        {
            model = Result<string>.Error(ex);
        }

    result:
        return model;
    }

    public async Task<Result<AgentDto>> SearchAgentAsync(int id)
    {
        Result<AgentDto> model = null;
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
                model = Result<AgentDto>.Error("User Not Found");
                goto result;
            }
            model = Result<AgentDto>.Success(agent);
        }
        catch (Exception ex)
        {
            model = Result<AgentDto>.Error(ex);
        }
    result:
        return model;
    }

    public async Task<Result<AgentListResponseModel>> SearchAgentByNameAsync(string? name, int pageNumber, int pageSize)
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
            int rowCount = _db.Agents.Count();
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
            return model;
        }
        catch (Exception ex)
        {
            model = Result<AgentListResponseModel>.Error(ex);
        }
        return model;
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

    public async Task<Result<AgentListResponseModel>> SearchAgentByNameAndLocationAsync(string name, string location, int pageNumber, int pageSize)
    {
        Result<AgentListResponseModel> model = null;
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

            AgentListResponseModel data = new AgentListResponseModel
            {
                PageCount = pageCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                AgentList = agents
            };
            model = Result<AgentListResponseModel>.Success(data);
        }
        catch (Exception ex)
        {
            model = Result<AgentListResponseModel>.Error(ex);
        }
        return model;

    }
}