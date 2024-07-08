using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using REMS.Modules.Features.Agent;

namespace REMS.BackendApi.Features.Agent
{
    [Route("api/v1/agents")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        private readonly BL_Agent _blAgent;

        public AgentController(BL_Agent blAgent)
        {
            _blAgent = blAgent;
        }
    }
}
