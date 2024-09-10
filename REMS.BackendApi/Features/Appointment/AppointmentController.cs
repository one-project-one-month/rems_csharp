using Microsoft.AspNetCore.Authorization;

namespace REMS.BackendApi.Features.Appointment;

[Route("api/v1/appointments")]
[ApiController]
public class AppointmentController : ControllerBase
{
    private readonly BL_Appointment _blAppointment;

    public AppointmentController(BL_Appointment blAppointment)
    {
        _blAppointment = blAppointment;
    }

    [HttpPost]
    public async Task<IActionResult> PostAppointment(AppointmentRequestModel requestModel)
    {
        try
        {
            var response = await _blAppointment.CreateAppointmentAsync(requestModel);
            if (response.IsError) return BadRequest(response);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAppointment(int id)
    {
        try
        {
            var response = await _blAppointment.DeleteAppointmentAsync(id);
            if (response.IsError)
                return BadRequest(response);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
        }
    }

    [HttpGet("property/{id}/{pageNo}/{pageSize}")]
    public async Task<IActionResult> GetAppointmentByPropertyIdAsync(int id, int pageNo = 1, int pageSize = 10)
    {
        try
        {
            var response = await _blAppointment.GetAppointmentByPropertyIdAsycn(id, pageNo, pageSize);
            if (response.IsError)
                return BadRequest(response);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateAppointment(int id, AppointmentRequestModel requestModel)
    {
        try
        {
            var response = await _blAppointment.UpdateAppointmentAsync(id, requestModel);
            if (response.IsError)
                return BadRequest(response);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex);
        }
    }

    [HttpGet("client/{id}/{pageNo}/{pageSize}", Name = "GetAppointmentByClientId")]
    public async Task<IActionResult> GetAppointmentByClientId(int id, int pageNo = 1, int pageSize = 10)
    {
        try
        {
            var response = await _blAppointment.GetAppointmentByClientId(id, pageNo, pageSize);
            if (response.IsError)
                return BadRequest(response);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
        }
    }
    [Authorize(Roles = "Admin")]
    [HttpGet("admin/{pageNo}/{pageSize}", Name = "GetAllAppointmentsForAdmin")]
    public async Task<IActionResult> GetAllAppointmentsForAdmin(int pageNo = 1, int pageSize = 10)
    {
        try
        {
            var response = await _blAppointment.GetAllAppointments(pageNo, pageSize);
            if (response.IsError)
                return BadRequest(response);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
        }
    }
}