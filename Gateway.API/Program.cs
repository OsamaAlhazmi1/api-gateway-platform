
using System.Threading.RateLimiting;
using Gateway.API.Auth;
using Gateway.API.Data;
using Gateway.API.Endpoints;
using Gateway.API.Services;
using Microsoft.EntityFrameworkCore;
using Yarp.ReverseProxy.Configuration;
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddValidation();

    builder.Services
        .AddReverseProxy()
        .Services
        .AddSingleton<DynamicProxyConfigProvider>();

    builder.Services.AddSingleton<IProxyConfigProvider>(sp =>
        sp.GetRequiredService<DynamicProxyConfigProvider>());

    builder.Services.AddDbContext<GatewayDbContext>(options =>
        options.UseNpgsql(
            builder.Configuration.GetConnectionString("GatewayDb")));
    builder.Services.AddScoped<ApiDefinitionService>();
    builder.Services.AddScoped<ApiClientServices>();

    builder.Services.AddJwtAuthentication(builder.Configuration);
    builder.Services.AddScoped<JwtTokenService>();
    builder.Services.AddRateLimiter(options =>
    {
        options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

        options.AddPolicy("ProxyPerApiClient", context =>
        {
            var clientId = context.User.FindFirst("ApiClientId")?.Value
                ?? "unknown-client";

            return RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: clientId,
                factory: _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 10,
                    Window = TimeSpan.FromMinutes(1),
                    QueueLimit = 0,
                    AutoReplenishment = true
                });
        });
    });

    var app = builder.Build();

    app.UseAuthentication();
    app.UseAuthorization();
    app.MigrateDb();
    app.MapManagementEndpoint();
    app.UseRateLimiter();
    app.MapReverseProxy()
        .RequireAuthorization("ApiKeyPolicy")
        .RequireRateLimiting("ProxyPerApiClient");
    app.MapAuthenticationEndpoints();
    app.Run();


}
catch (Exception ex)
{
    Console.Write(ex.Message);

}

