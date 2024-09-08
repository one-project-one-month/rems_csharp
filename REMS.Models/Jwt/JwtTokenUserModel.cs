namespace REMS.Models.Jwt;

public class JwtTokenUserModel
{
    public string UserId { get; set; }

    public string SessionId { get; set; }

    public string Role { get; set; }

    public DateTime TokenExpired { get; set; }
}