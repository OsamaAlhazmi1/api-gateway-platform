
using System.Globalization;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace Gateway.API.Services;

public static class RateLimitingExtensions
{
    public static IServiceCollection AddGatewayRateLimiting(
        this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode =
                StatusCodes.Status429TooManyRequests;

            options.AddPolicy("ProxyPerApiClient", context =>
            {
                var clientId = context.User
                    .FindFirst("ApiClientId")?.Value
                    ?? "unknown-client";

                var rateLimitValue = context.User
                    .FindFirst("RateLimitPerMinute")?.Value;

                var permitLimit = int.TryParse(
                    rateLimitValue,
                    NumberStyles.None,
                    CultureInfo.InvariantCulture,
                    out var configuredLimit)
                    ? configuredLimit
                    : 10;

                permitLimit = Math.Max(1, permitLimit);

                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: clientId,
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = permitLimit,
                        Window = TimeSpan.FromMinutes(1),
                        QueueLimit = 0,
                        AutoReplenishment = true
                    });
            });
        });

        return services;
    }
}
