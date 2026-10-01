using Gateway.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
var app = builder.Build();



app.MapManagementEndpoint();
app.MapReverseProxy();

app.Run();
