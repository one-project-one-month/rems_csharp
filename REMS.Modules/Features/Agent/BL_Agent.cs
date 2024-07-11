namespace REMS.Modules.Features.Agent;

public class BL_Agent
{
    private readonly DA_Agent _daAgent;

    public BL_Agent(DA_Agent daAgent)
    {
        _daAgent = daAgent;
    }

    public async Task<MessageResponseModel> CreateAgentAsync(AgentRequestModel requestModel)
    {
        var response = await _daAgent.CreateAgentAsync(requestModel);
        return response;
    }

    public async Task<MessageResponseModel> UpdateAgentAsync(int id, AgentRequestModel requestModel)
    {
        var response = await _daAgent.UpdateAgentAsync(id, requestModel);
        return response;
    }

    public async Task<MessageResponseModel> DeleteAgentAsync(int id)
    {
        var response = await _daAgent.DeleteAgentAsync(id);
        return response;
    }

    public async Task<AgentResponseModel> SearchAgentAsync(int id)
    {
        return await _daAgent.SearchAgentAsync(id);
    }

    public async Task<MessageResponseModel> LoginAgentAsync(AgentLoginRequestModel agentLoginInfo)
    {
        return await _daAgent.LoginAgentAsync(agentLoginInfo);
    }

    public async Task<AgentListResponseModel> SearchAgentByNameAsync(string name,int pageNumber,int pageSize)
    {
        if (pageNumber<1 || pageSize < 1)
        {
            AgentListResponseModel model = new AgentListResponseModel();
            model.Status = "Page Number or Page Size Can't be less than 1";
            return model;
        }
        return await _daAgent.SearchAgentByNameAsync(name, pageNumber, pageSize);
    }

    public async Task<AgentListResponseModel> SearchAgentByNameAndLocationAsync(SearchAgentRequestModel _searchAgent)
    {
        AgentListResponseModel _agentList = new AgentListResponseModel();
        if (_searchAgent.PageNumber < 1 || _searchAgent.PageSize < 1)
        {
            _agentList.Status = "Page Number or Page Size Can't be less than 1";
            return _agentList;
        }
        if (!string.IsNullOrEmpty(_searchAgent.Address))
        {
            _agentList =
                await _daAgent.SearchAgentByNameAndLocationAsync(_searchAgent.AgentName ?? "", _searchAgent.Address,_searchAgent.PageNumber,_searchAgent.PageSize);
        }
        else
        {
            _agentList = await _daAgent.SearchAgentByNameAsync(_searchAgent.AgentName ?? "", _searchAgent.PageNumber, _searchAgent.PageSize);
        }

        return _agentList;
    }
}