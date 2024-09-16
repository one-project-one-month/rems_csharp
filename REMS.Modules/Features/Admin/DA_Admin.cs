namespace REMS.Modules.Features.Admin;

public class DA_Admin
{
    private readonly AppDbContext _db;

    public DA_Admin(AppDbContext dbContext)
    {
        _db = dbContext;
    }

    public async Task<Result<AdminResponseModel>> CreateAdmin(AdminRequestModel adminRequest)
    {
        Result<AdminResponseModel> model = null;
        try
        {
            await IsEmailAlreadyExist(adminRequest.Email);

            var user = adminRequest.Change();

            user.Role = "Admin";

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

            var adminResponse = user.Change();

            model = Result<AdminResponseModel>.Success(adminResponse);
            return model;
        }
        catch (Exception ex)
        {
            model = Result<AdminResponseModel>.Error(ex.Message);
            return model;
        }
    }


    private async Task IsEmailAlreadyExist(string email)
    {
        bool emailExists = await _db.Users.AnyAsync(user => user.Email == email);
        if (emailExists)
        {
            throw new InvalidOperationException("An account with this email already exists.");
        }
    }


}
