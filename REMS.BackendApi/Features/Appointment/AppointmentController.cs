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
                var response=await _blAppointment.DeleteAppointmentAsync(id);
                if (response.IsError)
                    return BadRequest(response);
                return Ok(response);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.ToString());
            }
        }

        [HttpGet("{id}/{pageNo}/{pageSize}")]
        public async Task<IActionResult> GetAppointmentByAgentId(int id,int pageNo,int pageSize)
        {
            try
            {
                var response = await _blAppointment.GetAppointmentByAgentIdAsync(id, pageNo, pageSize);
                if (response.messageResponse!.IsError)
                    return BadRequest(response);
                return Ok(response);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
    }
}
