namespace REMS.BackendApi.Features.Client;

[Route("api/v1/clients")]
[ApiController]
// [Authorize(Roles = "Agent")]
public class ClientController : ControllerBase
{
    private readonly BL_Client _blClient;

    public ClientController(BL_Client blClient)
    {
        _blClient = blClient;
    }

    [HttpGet("{pageNo}/{pageSize}")]
    public async Task<IActionResult> GetClients(string? firstName, string? lastName, string? email, string? phone, int pageNo = 1, int pageSize = 10)
    {
        try
        {
            if (pageNo < 1 || pageSize < 1)
            {
                return BadRequest("PageNo or PageSize cannot be less than 1");
            }
            var response = await _blClient.GetClients(firstName, lastName, email, phone, pageNo, pageSize);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetClientById(int id)
    {
        try
        {
            var responseModel = await _blClient.GetClientById(id);
            return Ok(responseModel);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
        }
    }

    [HttpPost]
    public async Task<IActionResult> PostClient(ClientRequestModel requestModel)
    {
        try
        {
            var response = await _blClient.CreateClient(requestModel);
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
    public async Task<IActionResult> UpdateClient(int id, ClientRequestModel requestModel)
    {
        try
        {
            var response = await _blClient.UpdateClient(id, requestModel);
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        try
        {
            var response = await _blClient.DeleteClient(id);
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
