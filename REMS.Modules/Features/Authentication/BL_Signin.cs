using System.Globalization;
using System.Text.RegularExpressions;
using REMS.Models;

namespace REMS.Modules.Features.Authentication;

public class BL_Signin
{
    private readonly DA_Signin _daSignin;

    public BL_Signin(DA_Signin daSignin)
    {
        _daSignin = daSignin;
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
                string domainName = idn.GetAscii(match.Groups[2].Value);
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
}