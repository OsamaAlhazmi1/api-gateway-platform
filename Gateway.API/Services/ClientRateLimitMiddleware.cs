
using System.Globalization;
using Gateway.API.Services;
using Yarp.ReverseProxy.Configuration;

namespace Gateway.API.Services;

public static class ClientRateLimitMiddleware
{
    // YARP's proxy-only pipeline.
    public static IReverseProxyApplicationBuilder UseClientRateLimiting(
        this IReverseProxyApplicationBuilder proxyPipeline)
    {
        proxyPipeline.Use(async (context, next) =>
        {
            
            var clientIdValue = context.User
                .FindFirst("ApiClientId")?.Value;

            var rateLimitValue = context.User
                .FindFirst("RateLimitPerMinute")?.Value;

            // Stop if the client ID or limit is missing or invalid.
            if (!int.TryParse(clientIdValue, out var clientId) ||
                !int.TryParse(
                    rateLimitValue,
                    NumberStyles.None,
                    CultureInfo.InvariantCulture,
                    out var permitLimit) ||
                clientId <= 0 ||
                permitLimit <= 0)
            {
                context.Response.StatusCode =
                    StatusCodes.Status401Unauthorized;

                return;
            }

            var manager = context.RequestServices
                .GetRequiredService<ClientRateLimitManager>();

            var allowed = manager.TryAcquirePermit(
                clientId,
                permitLimit);

          
            if (!allowed)
            {
                context.Response.StatusCode =
                    StatusCodes.Status429TooManyRequests;

                await context.Response.WriteAsJsonAsync(new
                {
                    success = false,
                    message = "Rate limit exceeded."
                });

                return;
            }

            // The request is allowed. Continue through YARP's pipeline.
            await next();
        });

        return proxyPipeline;
    }
}
