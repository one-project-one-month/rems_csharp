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

    
    [HttpGet("{pageNo}/{pageSize}")]
    public async Task<IActionResult> GetProperties(
                                                int? agentId, string? address, string? city,
                                                string? state, string? zipCode,
                                                string? propertyType, decimal? minPrice,
                                                decimal? maxPrice, decimal? size,
                                                int? numberOfBedrooms, int? numberOfBathrooms,
                                                int? yearBuilt, string? availabilityType,
                                                int? minRentalPeriod, string? approvedBy,
                                                DateTime? addDate, DateTime? editDate,
                                                string? propertyStatus,
                                                int pageNo =1, int pageSize=10
                                                )
    {
        try
        {
            
            if (!string.IsNullOrWhiteSpace(propertyStatus) && !IsValidPropertyStatus(propertyStatus))
            {
                return BadRequest($"Invalid Status; Status should be one of the following: " +
                    $"{string.Join(", ", Enum.GetNames(typeof(PropertyStatus)))}");
            }
            if (!string.IsNullOrWhiteSpace(propertyType) && !IsValidPropertyType(propertyType))
            {
                return BadRequest($"Invalid Type; Property Type should be one of the following: " +
                    $"{string.Join(", ", Enum.GetNames(typeof(PropertyType)))}");
            }

            if (!string.IsNullOrWhiteSpace(availabilityType) && !IsValidPropertyAvailiableType(availabilityType))
            {
                return BadRequest($"Invalid Availability Type; Property Availability Type should be one of the following" +
                    $": {string.Join(", ", Enum.GetNames(typeof(PropertyAvailiableType)))}");
            }

            if (pageNo < 1 || pageSize < 1)
            {
                return BadRequest("PageNo or PageSize cannot be less than 1");
            }

            var response = await _blProperties.GetProperties(agentId, address, city, state,
                                                             zipCode, propertyType,
                                                             minPrice, maxPrice,size,
                                                             numberOfBedrooms, numberOfBathrooms,
                                                             yearBuilt, availabilityType,
                                                             minRentalPeriod, approvedBy,
                                                             addDate, editDate,
                                                             propertyStatus, pageNo, pageSize);

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

    [HttpPut("Update/{propertyId}")]
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