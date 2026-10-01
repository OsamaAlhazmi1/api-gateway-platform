using Gateway.API.DTOs;
using Gateway.API.Models;

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


        managementGroup.MapGet("/apis", () =>
        {
            var apiList = new List<ApiDefinition>
            {
                new ApiDefinition()
                {
                    Id = 1 ,
                    Name = "User API",
                    RoutePrefix = "/users/*",
                    DestinationAddress = "http://localhost:5024/"
                },
                new ApiDefinition()
                {
                    Id = 2,
                    Name = "Order API",
                    RoutePrefix = "/orders/*",
                    DestinationAddress = "http://localhost:5297"

                }


            };
            var reponse = apiList.Select(api=> new ApiDefinitionResponseDTOs(
                Id : api.Id,
                Name: api.Name,
                RoutePrefix: api.RoutePrefix
            ));
            
            return Results.Ok(reponse);
        });
    }
}
