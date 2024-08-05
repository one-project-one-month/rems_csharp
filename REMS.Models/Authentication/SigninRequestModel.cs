namespace REMS.Models.Authentication;

public record class SigninRequestModel(string Email, string Password);

public record class SigninResponseModel(string AccessToken, string Role);