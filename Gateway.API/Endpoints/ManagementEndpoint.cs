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
        var managementGroup = app.MapGroup("/api/management").RequireAuthorization(policy => policy.RequireRole("admin"));

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


        managementGroup.MapPost("/apis", async (ApiDefinitionService service,
        CreateApiDefinitionDTO dto,
        DynamicProxyConfigProvider proxyConfigProvider) =>
        {

            var newApi = await service.CreateAsync(dto);

            if (newApi == null)
                return Results.Conflict($"An API ({dto.Name}) with the same name or route prefix ({dto.RoutePrefix}) already exists");

            proxyConfigProvider.Reload();

            var response = new ApiDefinitionResponseDTO(
                Id: newApi.Id,
                Name: newApi.Name,
                RoutePrefix: newApi.RoutePrefix,
                DownstreamPath: newApi.DownstreamPath
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


        managementGroup.MapPut("/apis/{id}", async (int id,
        ApiDefinitionService service,
        UpdateApiDefinitionDTO dto,
        DynamicProxyConfigProvider proxyConfigProvider) =>
        {

            var api = await service.UpdateAsync(id, dto);

            if (api == null)
                return Results.NotFound($"API with {id} Not Found");

            proxyConfigProvider.Reload();

            var response = new ApiDefinitionResponseDTO(
                Id: api.Id,
                Name: api.Name,
                RoutePrefix: api.RoutePrefix,
                DownstreamPath: api.DownstreamPath
            );

            return Results.Ok(response);

        });

        managementGroup.MapDelete("/apis/{id}", async (
        int id,
        ApiDefinitionService service,
        DynamicProxyConfigProvider proxyConfigProvider) =>
        {
            var deleted = await service.DeleteAsync(id);

            if (!deleted)
                return Results.NotFound($"API with {id} Not Found");
            proxyConfigProvider.Reload();

            return Results.Ok($"API with Id {id} has been Removed");

        });

        managementGroup.MapPost("/clients/", async (CreateApiClientDTO dto, ApiClientServices services) =>
        {
            var (client, apiKey) = await services.CreateAsync(dto.Name);

            var response = new ApiClientResponseDTO(
                Id: client.Id,
                Name: client.Name,
                ApiKey: apiKey,
                IsActive: client.IsActive,
                CreatedAt: client.CreatedAt
            );

            return Results.Ok(response);

        });

        managementGroup.MapGet("/clients", async (ApiClientServices services) =>
        {
            var ClinetList = await services.GetAllAsync();

            return Results.Ok(ClinetList);
        });

        managementGroup.MapDelete("/clients/{id}", async (int id, ApiClientServices services) =>
        {
            var clinet = await services.DeleteAsync(id);

            if (!clinet)
                return ApiResponse.Fail($"API Clinet with ID : {id} Not Found", 400);
            else
                return ApiResponse.Success($"API clinet with ID : {id} is deleted", 200);

        });

        managementGroup.MapPatch("/clients/{id}/deactivate", async (int id, ApiClientServices services) =>
        {
            var client = await services.DeactivateApiKeyAsync(id);
            if (!client)
                return ApiResponse.Fail($"API Clinet with ID : {id} Not Found", 404);

            return ApiResponse.Success($"API key for clinet with ID : {id} is deactivate ", 200);

        });
        managementGroup.MapPatch("/clients/{id}/activate", async (int id, ApiClientServices services) =>
        {
            var client = await services.ActivateApiKeyAsync(id);
            if (!client)
                return ApiResponse.Fail($"API Clinet with ID : {id} Not Found", 404);

            return ApiResponse.Success($"API key for clinet with ID : {id} is activate ", 200);

        });
    }

}
