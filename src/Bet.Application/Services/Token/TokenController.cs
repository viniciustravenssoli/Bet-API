

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Bet.Application.Services.Token;
public class TokenController
{
    private const string EmailAlias = "eml";
    private readonly double _tokenExpiredInMinutes;
    private readonly string _tokenKey;

    public TokenController(double tokenExpiredInMinutes, string tokenKey)
    {
        _tokenExpiredInMinutes = tokenExpiredInMinutes;
        _tokenKey = tokenKey;
    }

    public string GenerateToken(string userEmail, string role)
    {
        var claims = new List<Claim>
        {
            new Claim(EmailAlias, userEmail),
            new Claim(ClaimTypes.Role, role)
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_tokenExpiredInMinutes),
            SigningCredentials = new SigningCredentials(SymmetricKey(), SecurityAlgorithms.HmacSha256Signature)
        };

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);
    }

    public ClaimsPrincipal Validate(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var parametersValidator = new TokenValidationParameters
        {
            RequireExpirationTime = true,
            IssuerSigningKey = SymmetricKey(),
            ClockSkew = new TimeSpan(0),
            ValidateIssuer = false,
            ValidateAudience = false,
        };

        var claims = tokenHandler.ValidateToken(token, parametersValidator, out _);

        return claims;
    }

    public string GetEmail(string token)
    {
        var claims = Validate(token);

        return claims.FindFirst(EmailAlias).Value;
    }

    private SymmetricSecurityKey SymmetricKey()
    {
        var symetricKey = Convert.FromBase64String(_tokenKey);
        return new SymmetricSecurityKey(symetricKey);
    }
}
