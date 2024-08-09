namespace REMS.BackendApi.Features.Appointment
{
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

        [HttpGet("{propertyId}/{pageNo}/{pageSize}")]
        public async Task<IActionResult> GetAppointmentByAgentId(int propertyId, int pageNo, int pageSize)
        {
            try
            {
                var response = await _blAppointment.GetAppointmentByPropertyIdAsycn(propertyId, pageNo, pageSize);
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
    }
}
