namespace REMS.BackendApi.Features.Agent;

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
            Result<AgentResponseModel> response = await _blAgent.CreateAgentAsync(requestModel);
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
            Result<object> response = await _blAgent.DeleteAgentAsync(userId);
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

    [HttpPatch("{userId}")]
    public async Task<IActionResult> UpdateAgent(int userId, AgentRequestModel requestModel)
    {
        try
        {
            Result<AgentResponseModel> response = await _blAgent.UpdateAgentAsync(userId, requestModel);
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

    [HttpGet("SearchUser/{id}", Name = "SearchUser")]
    public async Task<IActionResult> SearchUser(int id)
    {
        Result<AgentDto> agentList = await _blAgent.SearchAgentAsync(id);

        return Ok(agentList);
    }

    [HttpGet("SearchUserByName/{name}/{pageNumber}/{pageSize}", Name = "SearchUserByName")]
    public async Task<IActionResult> SearchUserByName(string name, int pageNumber, int pageSize)
    {
        Result<AgentListResponseModel> agentList = await _blAgent.SearchAgentByNameAsync(name, pageNumber, pageSize);

        return Ok(agentList);
    }

    [HttpPost("SearchAgentByNameAndLocation", Name = "SearchAgentByNameAndLocation")]
    public async Task<IActionResult> SearchAgentByNameAndLocation(SearchAgentRequestModel _searchAgentReqeustModel)
    {
        Result<AgentListResponseModel> agentList = await _blAgent.SearchAgentByNameAndLocationAsync(_searchAgentReqeustModel);

        return Ok(agentList);
    }
}