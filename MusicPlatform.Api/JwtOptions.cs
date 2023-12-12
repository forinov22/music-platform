using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MusicPlatform.Api;

public class JwtOptions
{
    public const string TokenValidationParameters = "TokenValidationParameters";

    public bool ValidateIssuer { get; set; }
    public bool ValidateAudience { get; set; }
    public bool ValidateLifetime { get; set; }
    public bool ValidateIssuerSigningKey { get; set; }
    public string ValidIssuer { get; set; } = null!;
    public string ValidAudience { get; set; } = null!;
    public string IssuerSigningKey { get; set; } = null!;

    public SymmetricSecurityKey GetSecretKey() {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(IssuerSigningKey));
    }
}
