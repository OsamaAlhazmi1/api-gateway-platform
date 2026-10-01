using Users.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapUsersEndpoints();



app.Run();
