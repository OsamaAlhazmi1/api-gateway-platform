
using System.Globalization;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace Gateway.API.Services;

public static class RateLimitingExtensions
{
    public static IServiceCollection AddGatewayRateLimiting(
        this IServiceCollection services)
    {
        services.AddSingleton<ClientRateLimitManager>();


        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode =
                StatusCodes.Status429TooManyRequests;

            options.AddPolicy("ProxyPerApiClient", context =>
            {
                var clientIdValue = context.User
                    .FindFirst("ApiClientId")?.Value
                    ?? "unknown-client";

                var clientId = int.Parse(clientIdValue);

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


                var manager = context.RequestServices.GetRequiredService<ClientRateLimitManager>();

                var limiter = manager.GetOrCreateLimiter(clientId, permitLimit) ;

                return RateLimitPartition.Get(clientId, _ => limiter);
            });
        });

        return services;
    }
}
