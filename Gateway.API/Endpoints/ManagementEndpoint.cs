using Gateway.API.Data;
using Gateway.API.DTOs;
using Gateway.API.DTOs.ApiDefinitionDTOs;
using Gateway.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Gateway.API.Endpoints;

public static class ManagementEndpoint
{
    const string GetAllApisEndpointName = "GetAllApis";
    const string GetApiByTheID = "GetApiByTheID";

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
            var apiList =  await context.ApiDefinitions.Select(api=> new ApiDefinitionResponseDTO(
                Id: api.Id,
                Name: api.Name,
                RoutePrefix: api.RoutePrefix
            )).AsNoTracking().ToListAsync();
 
            
            return Results.Ok(apiList);
        });

        managementGroup.MapPost("/apis",async (GatewayDbContext context , CreateApiDefinitionDTO dto) =>
        {


            var exist = await context.ApiDefinitions.FirstOrDefaultAsync
            (
            api=>api.Name==dto.Name||
            api.RoutePrefix == dto.RoutePrefix
            ); 

            if (exist!=null)
                return Results.Conflict($"An API ({dto.Name}) with the same name or route prefix ({dto.RoutePrefix}) already exists"); 
            
            var newApi = new ApiDefinition()
            {
                Name = dto.Name,
                RoutePrefix= dto.RoutePrefix,
                DestinationAddress= dto.DestinationAddress
            }; 
            await context.ApiDefinitions.AddAsync(newApi);
            await context.SaveChangesAsync(); 

            var response = new ApiDefinitionResponseDTO(
                Id: newApi.Id, 
                Name:newApi.Name, 
                RoutePrefix:newApi.RoutePrefix
            );

            return Results.Ok(response); 


        }).WithName(GetAllApisEndpointName);

        managementGroup.MapGet("/apis/{id}", async (int id , GatewayDbContext context) =>
        {
            var api = await context.ApiDefinitions.FirstOrDefaultAsync(api=>api.Id == id); 

            if (api == null)
                return Results.NotFound($"API with {id} Not Found");

            else
            {
                var response = new ApiDefinitionResponseDTO(
                    Id: api.Id,
                    Name:api.Name,
                    RoutePrefix:api.RoutePrefix
                );

                return Results.Ok(response); 
            }


        }).WithName(GetApiByTheID);

        managementGroup.MapPut("/apis/{id}", async (int id , GatewayDbContext context, UpdateApiDefinitionDTO dto) =>
        {
            var api = await context.ApiDefinitions.FirstOrDefaultAsync(p=>p.Id == id); 

            if (api == null)
                return Results.NotFound($"API with {id} Not Found");

            var exist = await context.ApiDefinitions.AnyAsync(existing =>
            existing.Id != id &&
            (existing.Name == dto.Name ||
             existing.RoutePrefix == dto.RoutePrefix)); 
            if (exist)
                return Results.Conflict($"An API ({dto.Name}) with the same name or route prefix ({dto.RoutePrefix}) already exists");

            else
            {
                api.Name= dto.Name; 
                api.RoutePrefix= dto.RoutePrefix; 
                api.DestinationAddress = dto.DestinationAddress; 
                await context.SaveChangesAsync(); 
                return Results.Ok(new ApiDefinitionResponseDTO(
                    Id: api.Id, 
                    Name: api.Name, 
                    RoutePrefix: api.RoutePrefix
                )); 
            }
            
        });

        managementGroup.MapDelete("/apis/{id}", async (int id , GatewayDbContext context) =>
        {
            var api = await context.ApiDefinitions.FirstOrDefaultAsync(p=>p.Id == id); 
            
            if (api == null)
                return Results.NotFound($"API with {id} Not Found");
            else
            {
                context.ApiDefinitions.Remove(api);
                await context.SaveChangesAsync(); 
                return Results.Ok($"API with Id {id} has been Removed");
            }
        });
    }
}
