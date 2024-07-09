namespace REMS.BackendApi.Features.Property;

[Route("api/v1/properties")]
[ApiController]
public class PropertyController : ControllerBase
{
    private readonly BL_Property _blProperties;

    public PropertyController(BL_Property blProperties)
    {
        _blProperties = blProperties;
    }

    [HttpGet]
    public async Task<IActionResult> GetProperties()
    {
        try
        {
            var response = await _blProperties.GetProperties();
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{pageNo}/{pageSize}")]
    public async Task<IActionResult> GetProperties(int pageNo, int pageSize)
    {
        try
        {
            var response = await _blProperties.GetProperties(pageNo, pageSize);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
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