using Microsoft.EntityFrameworkCore;

namespace Gateway.API.Data;

public static class DatabaseExtensions
{
    
    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var db = scope.ServiceProvider
            .GetRequiredService<GatewayDbContext>();

        db.Database.Migrate();
    }
}