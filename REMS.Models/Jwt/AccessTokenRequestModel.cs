namespace REMS.Models.Jwt;

public class AccessTokenRequestModel
{
    public string Role { get; set; }
    public string UserName { get; set; }
    public int UserId { get; set; }
}