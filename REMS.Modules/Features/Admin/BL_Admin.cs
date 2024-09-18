namespace REMS.Modules.Features.Admin;

public class BL_Admin
{
    private readonly DA_Admin _daAdmin;

    public BL_Admin(DA_Admin daAdmin)
    {
        _daAdmin = daAdmin;
    }

    public async Task<Result<AdminResponseModel>> CreateAdmin(AdminRequestModel adminRequest)
    {
        var response = await _daAdmin.CreateAdmin(adminRequest);
        return response;
    }
}
