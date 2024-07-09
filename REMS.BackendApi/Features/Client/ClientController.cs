using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using REMS.Models.Agent;
using REMS.Models.Client;
using REMS.Modules.Features.Agent;
using REMS.Modules.Features.Client;

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
