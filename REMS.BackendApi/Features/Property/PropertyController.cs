using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using REMS.Modules.Features.Property;

namespace REMS.BackendApi.Features.Property
{
    [Route("api/v1/properties")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly BL_Property _blProperties;

        public PropertyController(BL_Property blProperties)
        {
            _blProperties = blProperties;
        }

        [HttpGet("{propertyId}")]
        public async Task<IActionResult> GetPropertyById(int propertyId)
        {
            try
            {
                var response = await _blProperties.GetPropertyById(propertyId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
