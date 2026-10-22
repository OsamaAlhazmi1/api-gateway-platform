
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

    builder.Services.AddGatewayRateLimiting();
    builder.Services.AddDbContext<GatewayDbContext>(options =>
        options.UseNpgsql(
            builder.Configuration.GetConnectionString("GatewayDb")));
    builder.Services.AddScoped<ApiDefinitionService>();
    builder.Services.AddScoped<ApiClientServices>();
    builder.Services.AddScoped<ApiRequestLogService>();

    builder.Services.AddJwtAuthentication(builder.Configuration);
    builder.Services.AddScoped<JwtTokenService>();


    var app = builder.Build();

    app.UseAuthentication();
    app.UseAuthorization();
    app.MigrateDb();
    app.MapManagementEndpoint();
    app.MapReverseProxy(proxyPipeline =>
    {
        proxyPipeline.UseClientRateLimiting();
    }).RequireAuthorization("ApiKeyPolicy");
    // app.MapReverseProxy(proxyPipeLine => proxyPipeLine.GetClientIDValue());
    app.MapAuthenticationEndpoints();
    app.Run();


}
catch (Exception ex)
{
    Console.Write(ex.Message);

}

