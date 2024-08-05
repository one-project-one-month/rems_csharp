namespace REMS.Modules.Features.Authentication;

public class DA_Signin
{
    private readonly AppDbContext _db;
    private readonly JwtTokenService _jwtTokenService;

    public DA_Signin(AppDbContext db, JwtTokenService jwtTokenService)
    {
        _db = db;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Result<SigninResponseModel>> Signin(SigninRequestModel requestModel)
    {
        Result<SigninResponseModel> model;

        var user = await _db.Users
            .Where(us => us.Email == requestModel.Email
                         && us.Password == requestModel.Password)
            .FirstOrDefaultAsync();

        if (user is null)
        {
            model = Result<SigninResponseModel>.Error("Login Failed.");
            goto result;
        }

        string role = user.Role;
        var tokenModel = new AccessTokenRequestModel
        {
            UserName = user.Name,
            Role = user.Role
        };
        string accessToken = _jwtTokenService.GenerateAccessToken(tokenModel);

        await SaveLogin(user, accessToken);

        model = Result<SigninResponseModel>.Success(new SigninResponseModel(accessToken, role));

    result:
        return model;
    }

    private async Task SaveLogin(User data, string accessToken)
    {
        var login = new Login
        {
            UserId = data.UserId.ToString(),
            Role = data.Role,
            AccessToken = accessToken,
            Email = data.Email,
            LoginDate = DateTime.UtcNow
        };
        await _db.Logins.AddAsync(login);
        await _db.SaveChangesAsync();
    }

    public async Task<Result<string>> SignOut(string accessToken)
    {
        Result<string> model;

        var item = await _db.Logins
            .Where(l => l.AccessToken == accessToken)
            .FirstOrDefaultAsync();

        if (item is null)
        {
            model = Result<string>.Error("SignOut Fail: Access token not found.");
            goto result;
        }

        item.LogoutDate = DateTime.UtcNow;

        _db.Logins.Update(item);
        await _db.SaveChangesAsync();

        model = Result<string>.Success("SignOut successful.");
    result:
        return model;
    }
}