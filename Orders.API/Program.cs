using Orders.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapOrdersEndpoints();

app.Run();
