namespace Gateway.API.Endpoints;

public static class ManagementEndpoint
{
    public static void MapManagementEndpoint(this WebApplication app)
    {
        var managementGroup = app.MapGroup("/api/management");

        managementGroup.MapGet("/status", () =>
        {
            
            return Results.Ok(new
            {
                status = "running",
                Service = "API Gateway"
            });
        });
    }
}
