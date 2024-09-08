namespace REMS.Shared;

public class JwtTokenService
{
    private readonly AppDbContext _db;
    private readonly JwtTokenModel _token;

    public JwtTokenService(IOptionsMonitor<JwtTokenModel> token, AppDbContext db)
    {
        _db = db;
        _token = token.CurrentValue;
    }

    public async Task<AccessTokenModel> GenerateAccessToken(AccessTokenRequestModel requestModel)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var secret = _token.Key;
        var key = Encoding.ASCII.GetBytes(secret);
        var sessionId = Ulid.NewUlid().ToString();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Aud, _token.Audience),
                new Claim(JwtRegisteredClaimNames.Iss, _token.Issuer),
                new Claim("UserId", requestModel.UserId.ToString()),
                new Claim(ClaimTypes.Role, requestModel.Role),
                new Claim("SessionId", sessionId),
                new Claim("UserName", requestModel.UserName),
                new Claim("TokenExpired", DateTime.UtcNow.AddDays(1).ToString("O"))
            }),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var accessToken = tokenHandler.WriteToken(token);

        var refreshToken = _DevCode.GenerateRefreshToken();
        await AddSession(requestModel.UserId, refreshToken, accessToken);

        var model = new AccessTokenModel
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
        return model;
    }

    #region Add Refresh token to Session table

    public async Task AddSession(int userId, string refreshToken, string? accessTokens = null)
    {
        #region Add Session

        var tblSession = new Session
        {
            UserId = userId!.CheckEntityItem<int>(),
            RefreshToken = refreshToken,
            ExpiredTime = _DevCode.GetServerDateTime().AddMinutes(10),
            LastActiveTime = _DevCode.GetServerDateTime()
        };

        await _db.Sessions.AddAsync(tblSession);
        await _db.SaveChangesAsync();

        #endregion
    }

    #endregion

    #region Read AccessToken to Authorize

    public JwtTokenUserModel ReadAccessToken(string token)
    {
        var model = new JwtTokenUserModel();

        #region Decrypt Token

        var handler = new JwtSecurityTokenHandler();
        var decodedToken = handler.ReadJwtToken(token);

        #endregion

        #region Assign into Model

        var userId = decodedToken.Claims.FirstOrDefault(x => x.Type == "UserId") ??
                     throw new Exception("UserId is required.");
        var sessionId = decodedToken.Claims.FirstOrDefault(x => x.Type == "SessionId") ??
                        throw new Exception("SessionId is required.");
        var role = decodedToken.Claims.FirstOrDefault(x => x.Type == "role") ??
                   throw new Exception("Role is required.");
        var tokenExpire = decodedToken.Claims.FirstOrDefault(x => x.Type == "TokenExpired") ??
                          throw new Exception("TokenExpired is required.");

        model = new JwtTokenUserModel
        {
            UserId = userId.Value,
            Role = role.Value,
            SessionId = sessionId.Value,
            TokenExpired = tokenExpire.Value.CheckEntityItem<DateTime>()
        };
        if (model is null) throw new Exception("Unauthorized!");

        #endregion

        return model!;
    }

    #endregion
}