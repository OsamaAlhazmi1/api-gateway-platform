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


    var app = builder.Build();


    app.MigrateDb();
    app.MapManagementEndpoint();
    app.MapReverseProxy();
    app.Run();


}
catch (Exception ex)
{
    Console.Write(ex.Message);

}

