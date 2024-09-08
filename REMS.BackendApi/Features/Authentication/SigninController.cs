using Microsoft.AspNetCore.Authorization;

namespace REMS.BackendApi.Features.Authentication;

[Route("api/v1/")]
[ApiController]
public class SigninController : ControllerBase
{
    private readonly BL_Signin _blSignin;

    public SigninController(BL_Signin blSignin)
    {
        _blSignin = blSignin;
    }

    [HttpPost("Signin")]
    public async Task<IActionResult> Signin(SigninRequestModel requestModel)
    {
        try
        {
            var model = await _blSignin.Signin(requestModel);
            return Ok(model);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
        }
    }

    [HttpPost("Refresh-Token")]
    [Authorize]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestModel reqModel)
    {
        try
        {
            var request = new RefreshTokenModel
            {
                AccessToken = GetAccessToken(),
                RefreshToken = reqModel.RefreshToken
            };
            var model = await _blSignin.RefreshToken(request);
            return Ok(model);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
        }
    }

    [HttpPost("SignOut")]
    public async Task<IActionResult> SignOut(string accessToken)
    {
        try
        {
            var model = await _blSignin.SignOut(accessToken);
            return Ok(model);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
        }
    }

    private string GetAccessToken()
    {
        var token = Request.Headers["Authorization"].ToString();
        var accessToken = token.Substring("Bearer ".Length);
        return accessToken;
    }
}