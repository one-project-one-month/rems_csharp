using REMS.Models;
using REMS.Models.Property;

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
    public async Task<IActionResult> GetProperties(string? propertyStatus = "")
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(propertyStatus) && !IsValidPropertyStatus(propertyStatus))
            {
                return BadRequest($"Invalid Status; Status should be one of the following: {string.Join(", ", Enum.GetNames(typeof(PropertyStatus)))}");
            }

            var response = await _blProperties.GetProperties(propertyStatus);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{pageNo}/{pageSize}")]
    public async Task<IActionResult> GetProperties(int pageNo, int pageSize, string? propertyStatus = "")
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(propertyStatus) && !IsValidPropertyStatus(propertyStatus))
            {
                return BadRequest($"Invalid Status; Status should be one of the following: {string.Join(", ", Enum.GetNames(typeof(PropertyStatus)))}");
            }

            var response = await _blProperties.GetProperties(pageNo, pageSize, propertyStatus);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("agent/{agentId}")]
    public async Task<IActionResult> GetPropertiesByAgentId(int agentId, [FromQuery] string propertyStatus = "")
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(propertyStatus) && !IsValidPropertyStatus(propertyStatus))
            {
                return BadRequest($"Invalid Status; Status should be one of the following: {string.Join(", ", Enum.GetNames(typeof(PropertyStatus)))}");
            }

            var response = await _blProperties.GetPropertiesByAgentId(agentId, propertyStatus);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("agent/{agentId}/{pageNo}/{pageSize}")]
    public async Task<IActionResult> GetPropertiesByAgentId(int agentId, int pageNo, int pageSize, [FromQuery] string propertyStatus = "")
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(propertyStatus) && !IsValidPropertyStatus(propertyStatus))
            {
                return BadRequest($"Invalid Status; Status should be one of the following: {string.Join(", ", Enum.GetNames(typeof(PropertyStatus)))}");
            }

            var response = await _blProperties.GetPropertiesByAgentId(agentId, pageNo, pageSize, propertyStatus);
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

    [HttpPost]
    public async Task<IActionResult> CreateProperty([FromBody] PropertyRequestModel requestModel)
    {
        var validationResult = ValidatePropertyRequestModel(requestModel);
        if (validationResult != null)
        {
            return validationResult;
        }

        try
        {
            var response = await _blProperties.CreateProperty(requestModel);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{propertyId}")]
    public async Task<IActionResult> UpdateProperty(int propertyId, [FromBody] PropertyRequestModel requestModel)
    {
        if (propertyId < 1)
        {
            return BadRequest("Invalid Property Id");
        }

        var validationResult = ValidatePropertyRequestModel(requestModel);
        if (validationResult != null)
        {
            return validationResult;
        }

        try
        {
            var response = await _blProperties.UpdateProperty(propertyId, requestModel);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("ChangeStatus")]
    public async Task<IActionResult> ChangePropertyStatus(PropertyStatusChangeRequestModel requestModel)
    {
        if (requestModel == null)
        {
            return BadRequest("Reuqest cannot be null");
        }

        if (requestModel.PropertyId < 1)
        {
            return BadRequest("Invalid Property Id");
        }

        try
        {
            var result = await _blProperties.ChangePropertyStatus(requestModel);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpDelete("{propertyId}")]
    public async Task<IActionResult> DeleteProperty(int propertyId)
    {
        if (propertyId < 1)
        {
            return BadRequest("Invalid Property Id");
        }

        try
        {
            var result = await _blProperties.DeleteProperty(propertyId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    private IActionResult ValidatePropertyRequestModel(PropertyRequestModel requestModel)
    {
        if (requestModel == null)
        {
            return BadRequest("Request model cannot be null");
        }

        if (!IsValidPropertyType(requestModel.PropertyType))
        {
            return BadRequest($"Invalid Type; Property Type should be one of the following: {string.Join(", ", Enum.GetNames(typeof(PropertyType)))}");
        }

        if (!IsValidPropertyAvailiableType(requestModel.AvailiablityType))
        {
            return BadRequest($"Invalid Availability Type; Property Availability Type should be one of the following: {string.Join(", ", Enum.GetNames(typeof(PropertyAvailiableType)))}");
        }

        return null;
    }


    private static bool IsValidPropertyStatus(string status)
    {
        return Enum.TryParse<PropertyStatus>(status, out var parsedStatus) && Enum.IsDefined(typeof(PropertyStatus), parsedStatus);
    }

    private static bool IsValidPropertyType(string propertyType)
    {
        return Enum.TryParse<PropertyType>(propertyType, out var parsedPropertyType) && Enum.IsDefined(typeof(PropertyType), parsedPropertyType);
    }

    private static bool IsValidPropertyAvailiableType(string propertyAvailiableType)
    {
        return Enum.TryParse<PropertyAvailiableType>(propertyAvailiableType, out var paresedPropertyAvailiableType) && Enum.IsDefined(typeof(PropertyAvailiableType), paresedPropertyAvailiableType);
    }
}