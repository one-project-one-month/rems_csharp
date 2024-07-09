using REMS.Models.Agent;
using REMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace REMS.Modules.Features.Agent
{
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
        public async Task<AgentResponseModel> SearchAgentAsync(int id)
        {
            return await _daAgent.SearchAgentAsync(id);
        }
        public async Task<MessageResponseModel> LoginAgentAsync(AgentLoginRequestModel agentLoginInfo)
        {
            return await _daAgent.LoginAgentAsync(agentLoginInfo);
        }
        public async Task<AgentListResponseModel> SearchAgentByNameAsync(string name)
        {
            return await _daAgent.SearchAgentByNameAsync(name);
        }

        public async Task<AgentListResponseModel> SearchAgentByNameAndLocationAsync(SearchAgentRequestModel _searchAgent)
        {
            AgentListResponseModel _agentList = new AgentListResponseModel();
            if (!string.IsNullOrEmpty(_searchAgent.Address))
            {
                _agentList = await _daAgent.SearchAgentByNameAndLocationAsync(_searchAgent.AgentName ?? "", _searchAgent.Address);
            }
            else
            {
                _agentList= await _daAgent.SearchAgentByNameAsync(_searchAgent.AgentName ?? "");
            }
            return _agentList;
        }
    }
}
