using Azure;

namespace REMS.BackendApi.Features.Client;

[Route("api/v1/clients")]
[ApiController]
public class ClientController : ControllerBase
{
    private readonly BL_Client _blClient;

    public ClientController(BL_Client blClient)
    {
        _blClient = blClient;
    }

    [HttpGet]
    public async Task<IActionResult> GetClients()
    {
        try
        {
            var responseModel = await _blClient.GetClients();
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
            var response = await _blClient.CreateClientAsync(requestModel);
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
