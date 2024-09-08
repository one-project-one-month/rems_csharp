namespace REMS.Models.Authentication;

public record class SigninResponseModel(string role, AccessTokenModel tokens);