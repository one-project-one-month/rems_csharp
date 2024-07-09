namespace REMS.Modules.Features.Property;

public class BL_Property
{
    private readonly DA_Property _daProperty;

    public BL_Property(DA_Property daProperty)
    {
        _daProperty = daProperty;
    }

    public async Task<List<PropertyResponseModel>> GetProperties()
    {
        var response = await _daProperty.GetProperties();
        return response;
    }

    public async Task<PropertyListResponseModel> GetProperties(int pageNo, int pageSize)
    {
        if(pageNo < 1 || pageSize < 1)
        {
            throw new Exception("PageNo or PageSize Cannot be less than 1");
        }

        var response = await _daProperty.GetProperties(pageNo, pageSize);
        return response;
    }

    public async Task<PropertyResponseModel> GetPropertyById(int propertyId)
    {
        if (propertyId < 1)
        {
            throw new Exception("Invalid Property Id");
        }

        var response = await _daProperty.GetPropertyById(propertyId);
        return response;
    }
}