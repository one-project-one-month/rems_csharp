namespace REMS.Models.Jwt;

public class JwtTokenModel
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Key { get; set; }
}