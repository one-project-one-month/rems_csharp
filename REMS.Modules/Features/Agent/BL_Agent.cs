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

    public async Task<Result<AgentDto>> SearchAgentAsync(int id)
    {
        return await _daAgent.SearchAgentByUserIdAsync(id);
    }


    public async Task<Result<AgentListResponseModel>> SearchAgentByNameAsync(string name, int pageNumber, int pageSize)
    {
        if (pageNumber < 1 || pageSize < 1)
        {
            return Result<AgentListResponseModel>.Error("Page Number or Page Size Can't be less than 1");
        }
        return await _daAgent.SearchAgentByNameAsync(name, pageNumber, pageSize);
    }

    public async Task<Result<AgentListResponseModel>> SearchAgentByNameAndLocationAsync(SearchAgentRequestModel _searchAgent)
    {
        Result<AgentListResponseModel> model = null;
        if (_searchAgent.PageNumber < 1 || _searchAgent.PageSize < 1)
        {
            model = Result<AgentListResponseModel>.Error("Page Number or Page Size Can't be less than 1");
        }
        if (!string.IsNullOrEmpty(_searchAgent.Address))
        {
            model =
                await _daAgent.SearchAgentByNameAndLocationAsync(_searchAgent.AgentName ?? "", _searchAgent.Address, _searchAgent.PageNumber, _searchAgent.PageSize);
        }
        else
        {
            model = await _daAgent.SearchAgentByNameAsync(_searchAgent.AgentName ?? "", _searchAgent.PageNumber, _searchAgent.PageSize);
        }
        return model;
    }

    public async Task<Result<AgentListResponseModel>> AgentAll(int pageNumber, int pageSize)
    {
        return await _daAgent.AgentAllAsync(pageNumber,pageSize);
    }
}