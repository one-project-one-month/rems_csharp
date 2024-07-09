using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using REMS.Models.Agent;
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

        [HttpPost]
        public async Task<IActionResult> PostAgent(AgentRequestModel requestModel)
        {
            try
            {
                var response = await _blAgent.CreateAgentAsync(requestModel);
                if (response.IsError)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteAgent(int userId)
        {
            try
            {
                var response = await _blAgent.DeleteAgentAsync(userId);
                if (response.IsError)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAgent(int id, AgentRequestModel requestModel)
        {
            try
            {
                var response = await _blAgent.UpdateAgentAsync(id, requestModel);
                if (response.IsError)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
    }
}
