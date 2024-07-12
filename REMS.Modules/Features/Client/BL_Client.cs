using REMS.Modules.Features.Review;

namespace REMS.Modules.Features.Client;

public class BL_Client
{
    private readonly DA_Client _daClient;

    public BL_Client(DA_Client daClient)
    {
        _daClient = daClient;
    }

    public async Task<ClientListResponseModel> GetClients()
    {
        var response = await _daClient.GetClients();
        return response;
    }

    public async Task<ClientListResponseModel> GetClients(int pageNo, int pageSize)
    {
        if (pageNo < 1 || pageSize < 1)
        {
            throw new Exception("PageNo or PageSize Cannot be less than 1");
        }

        var response = await _daClient.GetClients(pageNo, pageSize);
        return response;
    }

    public async Task<ClientResponseModel> GetClientById(int id)
    {
        var responseModel = await _daClient.GetClientById(id);
        return responseModel;
    }

    public async Task<MessageResponseModel> CreateClientAsync(ClientRequestModel requestModel)
    {
        var response = await _daClient.CreateClientAsync(requestModel);
        return response;
    }

    public async Task<MessageResponseModel> UpdateClientAsync(int id, ClientRequestModel requestModel)
    {
        if (id <= 0) throw new Exception("id is null");
        var response = await _daClient.UpdateClientAsync(id, requestModel);
        return response;
    }
}