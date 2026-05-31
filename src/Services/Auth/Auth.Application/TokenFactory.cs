using System.Security.Claims;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Blocks.Core;
using Articles.Security;
using Microsoft.Extensions.Options;
using Auth.Domain.Users;

namespace Auth.Application;

public class TokenFactory
{
    private readonly JwtOptions _jwtSettings;

    public TokenFactory(IOptions<JwtOptions> jwtOptions) 
    {
        _jwtSettings = jwtOptions.Value;
    }

    public string GenerateJWTToken(string userId, string fullName, string email, IEnumerable<string> roles, IEnumerable<Claim> additionalClaims)
    {
        var claims = new[]
        {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Name, fullName),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToUnixEpochDate().ToString(), ClaimValueTypes.Integer64),

                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, fullName),
                new Claim(ClaimTypes.Email, email),
            }
        .Concat(roles.Select(r => new Claim(ClaimTypes.Role, r)))
        .Concat(additionalClaims);

        var secretKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(_jwtSettings.Secret));
        var signInCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var jwtToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            notBefore: DateTime.UtcNow,
            expires: _jwtSettings.Expiration,
            claims: claims,
            signingCredentials: signInCredentials
        );

        var encodedJwtToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        return encodedJwtToken;
    }

    public RefreshToken GenerateRefreshToken(string clientIpAddress)
    {
        using (var rng = RandomNumberGenerator.Create())
        {
            var randomBytes = new byte[64];
            rng.GetBytes(randomBytes);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                CreatedOn = DateTime.UtcNow,
                ExpiresOn = DateTime.UtcNow.AddDays(7),
                CreatedByIp = clientIpAddress
            };
        }
    }

}
