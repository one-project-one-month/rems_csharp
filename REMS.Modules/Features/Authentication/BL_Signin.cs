namespace REMS.Modules.Features.Authentication;

public class BL_Signin
{
    private readonly DA_Signin _daSignin;
    private readonly JwtTokenService _jwtTokenService;

    public BL_Signin(DA_Signin daSignin, JwtTokenService jwtTokenService)
    {
        _daSignin = daSignin;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Result<SigninResponseModel>> Signin(SigninRequestModel requestModel)
    {
        Result<SigninResponseModel> model;
        if (!IsValidEmail(requestModel.Email))
        {
            model = Result<SigninResponseModel>.Error("Invalid email format");
            goto result;
        }

        model = await _daSignin.Signin(requestModel);
    result:
        return model;
    }

    public async Task<Result<RefreshTokenResponseModel>> RefreshToken(RefreshTokenModel reqModel)
    {
        var model = new Result<RefreshTokenResponseModel>();

        #region Check Required Fields

        model = CheckRefreshTokenRequiredFields(reqModel);
        if (model.IsError) goto result;

        #endregion

        #region Read Access Token And Generate Refresh Token

        var tokenUser = _jwtTokenService.ReadAccessToken(reqModel.AccessToken);
        if (tokenUser is null)
        {
            model = Result<RefreshTokenResponseModel>.Error("Token user is null");
            goto result;
        }

        var userId = tokenUser.UserId!.CheckEntityItem<int>();
        model = await _daSignin.GetRefreshTokenByUserId(userId, reqModel.RefreshToken);
        if (model.IsError) goto result;

        var newRefreshToken = _DevCode.GenerateRefreshToken();

        #endregion

        if (newRefreshToken is null)
        {
            model = Result<RefreshTokenResponseModel>.Error("Generate Refresh Token Failed.");
            goto result;
        }

        #region Remove existing Refresh Token And Add new Refresh Token into tbl_session

        await _daSignin.DeleteUserRefreshToken(userId, reqModel.RefreshToken);
        await _jwtTokenService.AddSession(userId, newRefreshToken);

        #endregion

        model.Data = new RefreshTokenResponseModel
        {
            RefreshToken = newRefreshToken,
            UserId = userId
        };
        model = Result<RefreshTokenResponseModel>.Success(model.Data);

    result:
        return model;
    }


    public async Task<Result<string>> SignOut(string accessToken)
    {
        Result<string> model;
        model = await _daSignin.SignOut(accessToken);
        return model;
    }

    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            email = Regex.Replace(email, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));

            string DomainMapper(Match match)
            {
                var idn = new IdnMapping();
                var domainName = idn.GetAscii(match.Groups[2].Value);
                return match.Groups[1].Value + domainName;
            }
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
        catch (ArgumentException)
        {
            return false;
        }

        try
        {
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }

    private Result<RefreshTokenResponseModel> CheckRefreshTokenRequiredFields(RefreshTokenModel reqModel)
    {
        var model = new Result<RefreshTokenResponseModel>();
        if (reqModel.RefreshToken.IsNullOrEmpty())
        {
            model = Result<RefreshTokenResponseModel>.Error("Refresh Token is required.");
            goto result;
        }

        if (reqModel.AccessToken.IsNullOrEmpty())
        {
            model = Result<RefreshTokenResponseModel>.Error("Access Token is required.");
            goto result;
        }

        model = Result<RefreshTokenResponseModel>.SuccessResult();

    result:
        return model;
    }
}