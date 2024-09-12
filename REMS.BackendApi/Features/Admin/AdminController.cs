using Microsoft.AspNetCore.Authorization;

namespace REMS.BackendApi.Features.Admin;

[Route("api/v1/admins")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly BL_Admin _blAdmin;

    public AdminController(BL_Admin blAdmin)
    {
        _blAdmin = blAdmin;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAdmin(AdminRequestModel adminRequest)
    {
        try
        {
            var response = await _blAdmin.CreateAdmin(adminRequest);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
