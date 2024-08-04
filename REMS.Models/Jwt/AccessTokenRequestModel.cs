namespace REMS.Models.Jwt;

public class AccessTokenRequestModel
{
    public string Role { get; set; }
    public string UserName { get; set; }
    public DateTime TokenExpired { get; set; }
}