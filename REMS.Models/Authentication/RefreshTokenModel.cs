namespace REMS.Models.Authentication;

public class RefreshTokenModel
{
    public string? AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
public class RefreshTokenResponseModel
{
    public string RefreshToken { get; set; }
    public int? UserId { get; set; }
}

public class ValidateUserModel
{
    public bool IsValidateUser { get; set; }
    public string? UserId { get; set; }
    public string? RoleId { get; set; }
}