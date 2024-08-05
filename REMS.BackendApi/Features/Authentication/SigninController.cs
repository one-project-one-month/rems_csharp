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
}