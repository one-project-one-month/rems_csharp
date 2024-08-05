namespace REMS.Modules.Features.Property;

public class BL_Property
{
    private readonly DA_Property _daProperty;

    public BL_Property(DA_Property daProperty)
    {
        _daProperty = daProperty;
    }

    public async Task<Result<List<PropertyResponseModel>>> GetProperties()
    {
        var response = await _daProperty.GetProperties();
        return response;
    }

    public async Task<Result<PropertyListResponseModel>> GetProperties(int pageNo, int pageSize)
    {
        if (pageNo < 1 || pageSize < 1)
        {
            throw new Exception("PageNo or PageSize Cannot be less than 1");
        }

        var response = await _daProperty.GetProperties(pageNo, pageSize);
        return response;
    }

    public async Task<Result<List<PropertyResponseModel>>> GetPropertiesByAgentId(int agentId, string propertyStatus)
    {
        if (agentId < 1)
        {
            throw new Exception("Invalid Agent Id");
        }
        var response = await _daProperty.GetPropertiesByAgentId(agentId, propertyStatus);
        return response;
    }

    public async Task<Result<PropertyListResponseModel>> GetPropertiesByAgentId(int agentId, int pageNo, int pageSize, string propertyStatus)
    {
        if (agentId < 1)
        {
            throw new Exception("Invalid Agent Id");
        }
        if (pageNo < 1 || pageSize < 1)
        {
            throw new Exception("PageNo or PageSize Cannot be less than 1");
        }
        var response = await _daProperty.GetPropertiesByAgentId(agentId, pageNo, pageSize, propertyStatus);
        return response;
    }

    public async Task<Result<PropertyResponseModel>> GetPropertyById(int propertyId)
    {
        if (propertyId < 1)
        {
            throw new Exception("Invalid Property Id");
        }

        var response = await _daProperty.GetPropertyById(propertyId);
        return response;
    }

    public async Task<Result<PropertyResponseModel>> CreateProperty(PropertyRequestModel requestModel)
    {
        if (requestModel == null)
        {
            throw new ArgumentNullException(nameof(requestModel), "Request model cannot be null");
        }

        try
        {
            var response = await _daProperty.CreateProperty(requestModel);
            if (response == null)
            {
                throw new Exception("Failed to create property in data access layer");
            }

            return response;
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while creating the property: {ex.Message}");
        }
    }


    public async Task<Result<PropertyResponseModel>> UpdateProperty(int propertyId, PropertyRequestModel requestModel)
    {
        if (propertyId < 1)
        {
            throw new Exception("Invalid Property Id");
        }

        if (requestModel == null)
        {
            throw new ArgumentNullException(nameof(requestModel), "Request model cannot be null");
        }

        var response = await _daProperty.UpdateProperty(propertyId, requestModel);
        return response;
    }

    public async Task<Result<PropertyResponseModel>> ChangePropertyStatus(PropertyStatusChangeRequestModel requestModel)
    {
        if (requestModel.PropertyId < 1)
        {
            throw new Exception("Invalid Property Id");
        }
        var result = await _daProperty.ChangePropertyStatus(requestModel);
        return result;
    }

    public async Task<Result<object>> DeleteProperty(int propertyId)
    {
        if (propertyId < 1)
        {
            throw new Exception("Invalid Property Id");
        }

        var result = await _daProperty.DeleteProperty(propertyId);
        return result;
    }

}