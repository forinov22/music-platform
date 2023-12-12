using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MusicPlatform.Api.Services;

public interface ITokenService
{
    string GenerateToken(ClaimsIdentity claims);
}

public class TokenService : ITokenService
{
    private readonly JwtOptions jwtOptions;

    public TokenService(IOptions<JwtOptions> options)
    {
        jwtOptions = options.Value;
    }

    public string GenerateToken(ClaimsIdentity claims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(jwtOptions.IssuerSigningKey);

        var tokenDescriptor = new SecurityTokenDescriptor {
            Issuer = jwtOptions.ValidIssuer,
            Audience = jwtOptions.ValidAudience,
            Subject = claims,
            Expires = DateTime.UtcNow.AddDays(14),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                                        SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
