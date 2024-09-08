using REMS.Models.User;

namespace REMS.Modules.Features.Agent;

public class DA_Agent
{
    private readonly AppDbContext _db;

    public DA_Agent(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Result<AgentResponseModel>> CreateAgentAsync(AgentRequestModel requestModel)
    {
        Result<AgentResponseModel> response = null;
        try
        {
            await _db.Users.AddAsync(requestModel.ChangeUser());
            var result = await _db.SaveChangesAsync();
            if (result < 0) return Result<AgentResponseModel>.Error("Registration Fail");

            var user = await _db.Users
                .OrderByDescending(x => x.UserId)
                .AsNoTracking()
                .FirstAsync();
            requestModel.UserId = user.UserId;
            var agent = requestModel.ChangeAgent();
            await _db.Agents.AddAsync(agent);
            var addAgent = await _db.SaveChangesAsync();
            if (addAgent < 0) return Result<AgentResponseModel>.Error("Agent Register Fail");
            var agentResponse = new AgentResponseModel
            {
                Agent = agent.ChangeAgent(user)
            };
            response = Result<AgentResponseModel>.Success(agentResponse, "Agent Register Successfully");
        }
        catch (Exception ex)
        {
            response = Result<AgentResponseModel>.Error(ex);
        }

        return response;
    }

    public async Task<Result<AgentResponseModel>> UpdateAgentAsync(int id, AgentRequestModel requestModel)
    {
        Result<AgentResponseModel> response = null;
        try
        {
            var user = await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == id);
            var agent = await _db.Agents
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == id);
            if (user is null || agent is null) return Result<AgentResponseModel>.Error("User Not Found");
            if (!string.IsNullOrWhiteSpace(requestModel.UserName)) user.Name = requestModel.UserName;
            if (!string.IsNullOrWhiteSpace(requestModel.AgentName)) agent.AgencyName = requestModel.AgentName;
            if (!string.IsNullOrWhiteSpace(requestModel.LicenseNumber))
                agent.LicenseNumber = requestModel.LicenseNumber;
            if (!string.IsNullOrWhiteSpace(requestModel.Email)) user.Email = requestModel.Email;
            if (!string.IsNullOrWhiteSpace(requestModel.Password)) user.Password = requestModel.Password;
            if (!string.IsNullOrWhiteSpace(requestModel.Phone)) user.Phone = requestModel.Phone;
            if (!string.IsNullOrWhiteSpace(requestModel.Address)) agent.Address = requestModel.Address;
            _db.Entry(user).State = EntityState.Modified;
            _db.Entry(agent).State = EntityState.Modified;
            var result = await _db.SaveChangesAsync();
            if (result < 0) return Result<AgentResponseModel>.Error("Updating Fail");
            var agentResponse = new AgentResponseModel
            {
                Agent = agent.ChangeAgent(user)
            };
            response = Result<AgentResponseModel>.Success(agentResponse, "Successfully Update");
        }
        catch (Exception ex)
        {
            return Result<AgentResponseModel>.Error(ex);
        }

        return response;
    }

    public async Task<Result<object>> DeleteAgentAsync(int userId)
    {
        Result<object> response = null;
        try
        {
            var user = await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId);
            var agent = await _db.Agents
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if (user is null || agent is null) return Result<object>.Error("User Not Found ");

            _db.Users.Remove(user);
            _db.Entry(user).State = EntityState.Deleted;
            _db.Agents.Remove(agent);
            _db.Entry(agent).State = EntityState.Deleted;
            var result = await _db.SaveChangesAsync();
            response = result > 0
                ? Result<object>.Success(null, "Successfully Delete")
                : Result<object>.Error("Deleting Fail");
        }
        catch (Exception ex)
        {
            return Result<object>.Error(ex);
        }

        return response;
    }

    public async Task<Result<AgentDto>> SearchAgentByUserIdAsync(int id)
    {
        Result<AgentDto> model = null;
        try
        {
            var agent = await _db.Agents
                .Where(ag => ag.UserId == id)
                .Select(ag => new AgentDto
                {
                    AgentId = ag.AgentId,
                    UserId = ag.UserId,
                    AgencyName = ag.AgencyName,
                    LicenseNumber = ag.LicenseNumber,
                    Address = ag.Address
                })
                .FirstOrDefaultAsync();
            var user = await _db.Users
                .Where(x => x.UserId == id)
                .Select(n => new UserModel
                {
                    Phone = n.Phone,
                    Email = n.Email
                })
                .FirstOrDefaultAsync();
            if (agent is null || user is null)
            {
                model = Result<AgentDto>.Error("User Not Found");
                goto result;
            }

            agent.PhoneNumber = user.Phone;
            agent.Email = user.Email;
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
            var agents = await _db.Agents
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
                    Address = ag.Address
                })
                .ToListAsync();
            var rowCount = _db.Agents.Count();
            var pageCount = rowCount / pageSize;
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

    public async Task<Result<AgentListResponseModel>> SearchAgentByNameAsyncV2(string? name, int pageNumber,
        int pageSize)
    {
        Result<AgentListResponseModel> model = null;
        try
        {
            var agents = await _db.Agents
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
                    Address = ag.Address
                })
                .ToListAsync();
            var rowCount = await _db.Agents.CountAsync();
            var pageCount = rowCount / pageSize;
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

    public async Task<Result<AgentListResponseModel>> SearchAgentByNameAndLocationAsync(string name, string location,
        int pageNumber, int pageSize)
    {
        Result<AgentListResponseModel> model = null;
        try
        {
            var agents = await _db.Agents
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
                    Address = ag.Address
                })
                .ToListAsync();
            var rowCount = _db.Agents.Count();
            var pageCount = rowCount / pageSize;
            if (pageCount % pageSize > 0)
                pageCount++;

            var data = new AgentListResponseModel
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


    public async Task<Result<AgentListResponseModel>> AgentAllAsync(int pageNumber, int pageSize)
    {
        Result<AgentListResponseModel> model = null;
        try
        {
            List<AgentDto> agents = await (from ag in _db.Agents
                                           join _user in _db.Users on ag.UserId equals _user.UserId
                                           select new AgentDto
                                           {
                                               AgentId = ag.AgentId,
                                               UserId = ag.UserId,
                                               AgencyName = ag.AgencyName,
                                               LicenseNumber = ag.LicenseNumber,
                                               Email = _user.Email,
                                               PhoneNumber = _user.Phone,
                                               Address = ag.Address,
                                               Role = "agent"
                                           }).ToListAsync();
            var rowCount = _db.Agents.Count();
            var pageCount = rowCount / pageSize;
            if (pageCount % pageSize > 0)
                pageCount++;

            var data = new AgentListResponseModel
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