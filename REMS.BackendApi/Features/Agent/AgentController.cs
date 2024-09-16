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
            var response = await _blAgent.CreateAgentAsync(requestModel);
            if (response.IsError) return BadRequest(response);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAgent(int id)
    {
        try
        {
            var response = await _blAgent.DeleteAgentAsync(id);
            if (response.IsError) return BadRequest(response);
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
            if (response.IsError) return BadRequest(response);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
        }
    }

    [HttpGet("Users/{id}")]
    public async Task<IActionResult> SearchAgentByUserId(int id)
    {
        var agentList = await _blAgent.SearchAgentAsync(id);

        return Ok(agentList);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> SearchAgentById(int id)
    {
        var agentList = await _blAgent.SearchAgentById(id);

        return Ok(agentList);
    }

    [HttpGet("GetAgent")]
    public async Task<IActionResult> SearchUserByName(string agencyName, int pageNo= 1, int pageSize=10)
    {
        var agentList = await _blAgent.SearchAgentByNameAsync(agencyName, pageNo, pageSize);

        return Ok(agentList);
    }

    [HttpGet("agencyName/Location")]
    public async Task<IActionResult> SearchAgentByNameAndLocation(string? agencyName, string? location, int pageNo = 1, int pageSize = 10)
    {
        var agentList = await _blAgent.SearchAgentByNameAndLocation(agencyName, location, pageNo, pageSize);

        return Ok(agentList);
    }

    [HttpGet("{pageNumber}/{pageSize}", Name = "AgentAll")]
    public async Task<IActionResult> AgentAll(int pageNumber, int pageSize)
    {
        var agentList = await _blAgent.AgentAll(pageNumber, pageSize);

        return Ok(agentList);
    }
}