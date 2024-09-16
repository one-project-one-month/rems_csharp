namespace REMS.Modules.Features.Agent;

public class BL_Agent
{
    private readonly DA_Agent _daAgent;

    public BL_Agent(DA_Agent daAgent)
    {
        _daAgent = daAgent;
    }

    public async Task<Result<AgentResponseModel>> CreateAgentAsync(AgentRequestModel requestModel)
    {
        var response = await _daAgent.CreateAgentAsync(requestModel);
        return response;
    }

    public async Task<Result<AgentResponseModel>> UpdateAgentAsync(int id, AgentRequestModel requestModel)
    {
        var response = await _daAgent.UpdateAgentAsync(id, requestModel);
        return response;
    }

    public async Task<Result<object>> DeleteAgentAsync(int id)
    {
        var response = await _daAgent.DeleteAgentAsync(id);
        return response;
    }

    public async Task<Result<AgentDto>> SearchAgentAsync(int AgentId)
    {
        return await _daAgent.SearchAgentByUserIdAsync(AgentId);
    }

    public async Task<Result<AgentDto>> SearchAgentById(int AgentId)
    {
        return await _daAgent.SearchAgentById(AgentId);
    }

    public async Task<Result<AgentListResponseModel>> SearchAgentByNameAsync(string name, int pageNo,
        int pageSize)
    {
        if (pageNo < 1 || pageSize < 1)
            return Result<AgentListResponseModel>.Error("Page Number or Page Size Can't be less than 1");
        return await _daAgent.SearchAgentByNameAsync(name, pageNo, pageSize);
    }

    public async Task<Result<AgentListResponseModel>> SearchAgentByNameAndLocation(string agencyName, string location,
        int pageNo, int pageSize)
    {
        Result<AgentListResponseModel> model = null;
        model = await _daAgent.SearchAgentByNameAndLocation(agencyName, location, pageNo,
                pageSize);
        return model;
    }

    public async Task<Result<AgentListResponseModel>> AgentAll(int pageNumber, int pageSize)
    {
        return await _daAgent.AgentAllAsync(pageNumber, pageSize);
    }
}