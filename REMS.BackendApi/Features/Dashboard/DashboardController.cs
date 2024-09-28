namespace REMS.BackendApi.Features.Dashboard
{

    [Route("api/v1/Dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly BL_Dashboard _blDashboard;

        public DashboardController(BL_Dashboard blDashboard)
        {
            _blDashboard = blDashboard;
        }


        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {

            try
            {
                var response = await _blDashboard.GetDashboardAsync();
                if (response.IsError) return BadRequest(response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }

    }
}
