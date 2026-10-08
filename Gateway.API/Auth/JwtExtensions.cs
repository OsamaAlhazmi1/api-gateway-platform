using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Gateway.API.Auth;

public static class JwtExtensions
{
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSection = configuration.GetSection("Jwt")
            ?? throw new InvalidOperationException("JWT Section is not configured");
        
        var secretKey = jwtSection["Key"]
            ?? throw new InvalidOperationException("JWT SecretKey is not configured.");

        var issuer = jwtSection["Issuer"]
            ?? throw new InvalidOperationException("JWT issuer  is not configured");
            
        var audience = jwtSection["Audience"] 
            ?? throw new InvalidOperationException("JWT issuer  is not configured");
        
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {

                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = issuer,
                    ValidAudience = audience,

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            secretKey))
                };
            });

        services.AddAuthorization();

        return services;
    }
}