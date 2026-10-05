using Gateway.API.Data;
using Gateway.API.Endpoints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
builder.Services.AddDbContext<GatewayDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("GatewayDb")));



var app = builder.Build();


app.MigrateDb();
app.MapManagementEndpoint();
app.MapReverseProxy();

app.Run();
