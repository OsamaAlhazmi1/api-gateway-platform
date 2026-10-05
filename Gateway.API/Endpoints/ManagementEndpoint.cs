using Gateway.API.Data;
using Gateway.API.DTOs;
using Gateway.API.DTOs.ApiDefinitionDTOs;
using Gateway.API.Models;
using Microsoft.EntityFrameworkCore;

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


        managementGroup.MapGet("/apis",  async (GatewayDbContext context) =>
        {
            var apiList =  await context.ApiDefinitions.Select(api=> new ApiDefinitionResponseDTOs(
                Id: api.Id,
                Name: api.Name,
                RoutePrefix: api.RoutePrefix
            )).AsNoTracking().ToListAsync();
 
            
            return Results.Ok(apiList);
        });

        managementGroup.MapPost("/apis",async (GatewayDbContext context , CreateApiDefinitionDTOs dto) =>
        {
            var newApi = new ApiDefinition()
            {
                Name = dto.Name,
                RoutePrefix= dto.RoutePrefix,
                DestinationAddress= dto.DestinationAddress
            }; 
            await context.ApiDefinitions.AddAsync(newApi);
            await context.SaveChangesAsync(); 

            var response = new ApiDefinitionResponseDTOs(
                Id: newApi.Id, 
                Name:newApi.Name, 
                RoutePrefix:newApi.RoutePrefix
            );

            return Results.Ok(response); 


        });
    }
}
