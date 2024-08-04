namespace REMS.Shared;

public class JwtTokenService
{
    private readonly JwtTokenModel _token;

    public JwtTokenService(IOptionsMonitor<JwtTokenModel> token)
    {
        _token = token.CurrentValue;
    }

    public string GenerateAccessToken(AccessTokenRequestModel requestModel)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var secret = _token.Key;
        var key = Encoding.ASCII.GetBytes(secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Aud, _token.Audience),
                new Claim(JwtRegisteredClaimNames.Iss, _token.Issuer),
                new Claim(ClaimTypes.Role, requestModel.Role),
                new Claim(ClaimTypes.Name, requestModel.UserName),
                new Claim("TokenExpired", DateTime.UtcNow.AddDays(1).ToString("O")),
            }),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    // private AccessTokenRequestModel ReadToken(string token)
    // {
    //     var handler = new JwtSecurityTokenHandler();
    //     var decodedToken = handler.ReadJwtToken(token);
    //
    //     var item = decodedToken.Claims.FirstOrDefault(x => x.Type == "TokenExpired");
    //     DateTime tokenExpired = Convert.ToDateTime(item?.Value);
    //
    //     var role = decodedToken.Claims.FirstOrDefault(x => x.Type == "Role") ?? throw new Exception("Id is required.");
    //     var adminName = decodedToken.Claims.FirstOrDefault(x => x.Type == "AdminName") ??
    //                     throw new Exception("AdminName is required");
    //     var model = new AccessTokenRequestModel
    //     {
    //         Role = role.Value,
    //         UserName = adminName.Value,
    //         TokenExpired = tokenExpired,
    //     };
    //     return model;
    // }
}