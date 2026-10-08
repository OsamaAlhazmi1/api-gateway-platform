using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Gateway.API.Auth;

public class JwtTokenService(IConfiguration configuration)
{


    public string GenerateToken(string username, string role)
    {

        var jwtSection = configuration.GetSection("Jwt")
            ?? throw new InvalidOperationException("JWT Section is not configured");

        var secretKey = jwtSection["Key"]
            ?? throw new InvalidOperationException("JWT SecretKey is not configured.");

        var issuer = jwtSection["Issuer"]
            ?? throw new InvalidOperationException("JWT issuer  is not configured");

        var audience = jwtSection["Audience"]
            ?? throw new InvalidOperationException("JWT issuer  is not configured");
        
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, username),
            new(ClaimTypes.Name, username),
            new(ClaimTypes.Role, role)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretKey));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}