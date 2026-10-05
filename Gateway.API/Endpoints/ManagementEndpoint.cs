using Gateway.API.Data;
using Gateway.API.DTOs;
using Gateway.API.DTOs.ApiDefinitionDTOs;
using Gateway.API.Models;
using Gateway.API.Services;
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


        managementGroup.MapGet("/apis", async (ApiDefinitionService service) =>
        {
            var apiList = await service.GetAllAsync();

            return Results.Ok(apiList);
        });

        managementGroup.MapPost("/apis", async (ApiDefinitionService service, CreateApiDefinitionDTO dto) =>
        {
            var newApi = await service.CreateAsync(dto);

            if (newApi == null)
                return Results.Conflict($"An API ({dto.Name}) with the same name or route prefix ({dto.RoutePrefix}) already exists");

            var response = new ApiDefinitionResponseDTO(
                Id: newApi.Id,
                Name: newApi.Name,
                RoutePrefix: newApi.RoutePrefix
            );

            return Results.Ok(response);


        }).WithName(GetAllApisEndpointName);


        managementGroup.MapGet("/apis/{id}", async (int id, ApiDefinitionService service) =>
        {
            var api = await service.GetByIdAsync(id);

            if (api == null)
                return Results.NotFound($"API with {id} Not Found");

            else
            {
                return Results.Ok(api);
            }

        }).WithName(GetApiByTheID);


        managementGroup.MapPut("/apis/{id}", async (int id, ApiDefinitionService service, UpdateApiDefinitionDTO dto) =>
        {
            try
            {
                var api = await service.UpdateAsync(id, dto);

                if (api == null)
                    return Results.NotFound($"API with {id} Not Found");


                var response = new ApiDefinitionResponseDTO(
                    Id: api.Id,
                    Name: api.Name,
                    RoutePrefix: api.RoutePrefix
                );

                return Results.Ok(response);

            }
            catch (InvalidOperationException ex)
            {
                return Results.Conflict(ex.Message);
            }




        });

        managementGroup.MapDelete("/apis/{id}", async (int id, ApiDefinitionService service) =>
        {
            var deleted = await service.DeleteAsync(id);

            if (!deleted)
                return Results.NotFound($"API with {id} Not Found");


            return Results.Ok($"API with Id {id} has been Removed");

        });
    }
}
