namespace REMS.Modules.Features.Property;

public class BL_Property
{
    private readonly DA_Property _daProperty;

    public BL_Property(DA_Property daProperty)
    {
        _daProperty = daProperty;
    }

    public async Task<Result<PropertyListResponseModel>> GetProperties(int? agentId, string? address, string? city,
        string? state, string? zipCode,
        string? propertyType, decimal? minPrice,
        decimal? maxPrice,
        decimal? size, int? numberOfBedrooms,
        int? numberOfBathrooms,
        int? yearBuilt, string? availabilityType,
        int? minRentalPeriod,
        string? approvedBy, DateTime? addDate,
        DateTime? editDate,
        string? propertyStatus,
        int pageNo = 1, int pageSize = 10)
    {
        if (pageNo < 1 || pageSize < 1) throw new Exception("PageNo or PageSize Cannot be less than 1");

        var response = await _daProperty.GetProperties(agentId, address, city,
            state, zipCode, propertyType,
            minPrice, maxPrice, size,
            numberOfBedrooms, numberOfBathrooms,
            yearBuilt, availabilityType,
            minRentalPeriod, approvedBy,
            addDate, editDate,
            propertyStatus, pageNo, pageSize);

        return response;
    }

    public async Task<Result<PropertyResponseModel>> GetPropertyById(int propertyId)
    {
        if (propertyId < 1) throw new Exception("Invalid Property Id");

        var response = await _daProperty.GetPropertyById(propertyId);
        return response;
    }

    public async Task<Result<PropertyResponseModel>> CreateProperty(PropertyRequestModel requestModel)
    {
        if (requestModel == null) throw new ArgumentNullException(nameof(requestModel), "Request model cannot be null");

        try
        {
            var response = await _daProperty.CreateProperty(requestModel);
            if (response == null) throw new Exception("Failed to create property in data access layer");

            return response;
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while creating the property: {ex.Message}");
        }
    }

    public async Task<Result<PropertyResponseModel>> UpdateProperty(int propertyId, PropertyRequestModel requestModel)
    {
        if (propertyId < 1) throw new Exception("Invalid Property Id");

        if (requestModel == null) throw new ArgumentNullException(nameof(requestModel), "Request model cannot be null");

        var response = await _daProperty.UpdateProperty(propertyId, requestModel);
        return response;
    }

    public async Task<Result<PropertyResponseModel>> ChangePropertyStatus(PropertyStatusChangeRequestModel requestModel)
    {
        if (requestModel.PropertyId < 1) throw new Exception("Invalid Property Id");
        var result = await _daProperty.ChangePropertyStatus(requestModel);
        return result;
    }

    public async Task<Result<object>> DeleteProperty(int propertyId)
    {
        if (propertyId < 1) throw new Exception("Invalid Property Id");

        var result = await _daProperty.DeleteProperty(propertyId);
        return result;
    }
}