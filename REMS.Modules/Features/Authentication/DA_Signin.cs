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
            model = Result<SigninResponseModel>.Error("Username or Password is incorrect.");
            goto result;
        }

        var role = user.Role;
        var tokenModel = new AccessTokenRequestModel
        {
            UserName = user.Name,
            UserId = user.UserId,
            Role = user.Role
        };
        var token = await _jwtTokenService.GenerateAccessToken(tokenModel);

        await SaveLogin(user, token);

        model = Result<SigninResponseModel>.Success(new SigninResponseModel(role, token));

    result:
        return model;
    }

    private async Task SaveLogin(User data, AccessTokenModel reqModel)
    {
        var login = new Login
        {
            UserId = data.UserId.ToString(),
            Role = data.Role,
            AccessToken = reqModel.AccessToken,
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

    #region Refresh Token

    public async Task<Result<RefreshTokenResponseModel>> GetRefreshTokenByUserId(int userId, string oldRefreshToken)
    {
        var model = new Result<RefreshTokenResponseModel>();
        var session = await _db.Sessions.AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync();

        if (session is null)
        {
            model = Result<RefreshTokenResponseModel>.Error("No Session found.");
            goto result;
        }

        if (session.RefreshToken is null)
        {
            model = Result<RefreshTokenResponseModel>.Error("Invalid Session.");
            goto result;
        }

        if (session.RefreshToken != oldRefreshToken)
        {
            model = Result<RefreshTokenResponseModel>.Error("Invalid Refresh Token.");
            goto result;
        }

        var tokenObj = new RefreshTokenResponseModel
        {
            UserId = session.UserId,
            RefreshToken = session.RefreshToken
        };
        model = Result<RefreshTokenResponseModel>.Success(tokenObj);

    result:
        return model;
    }

    public async Task DeleteUserRefreshToken(long userId, string refreshToken)
    {
        var session = await _db.Sessions.AsNoTracking()
            .Where(x => x.UserId == userId && x.RefreshToken == refreshToken)
            .FirstOrDefaultAsync();

        if (session is null)
            throw new Exception("Session is null.");

        _db.Sessions.Remove(session);
        await _db.SaveChangesAsync();
    }

    #endregion
}