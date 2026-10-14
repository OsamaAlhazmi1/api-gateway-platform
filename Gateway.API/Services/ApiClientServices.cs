using Gateway.API.Data;
using Gateway.API.DTOs.ApiDefinitionDTOs;
using Gateway.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Gateway.API.Services;

public class ApiClientServices(GatewayDbContext context)
{

    
    public async Task<ApiClient?> CreateAsync(CreateApiClientDTO dto)
    {
        var exist = await context.ApiClients.FirstOrDefaultAsync(
        key => key.Name == dto.Name
        || key.ApiKey == dto.ApiKey);

        if (exist != null)
            return null;


        var newApiClient = new ApiClient()
        {
            Name = dto.Name,
            ApiKey = dto.ApiKey,
            IsActive = dto.IsActive,
            CreatedAt = dto.CreatedAt
        };
        await context.ApiClients.AddAsync(newApiClient);
        await context.SaveChangesAsync();

        return newApiClient;

    }

    public async Task<ApiClient?> UpdateAsync(int id, UpdateApiClientDTO dto)
    {
        var apiClient = await context.ApiClients.FirstOrDefaultAsync(c => c.Id == id);

        if (apiClient == null)
            return null;

        var exist = await context.ApiClients.AnyAsync(
        key => key.Name == dto.Name &&
        key.Id == id
        || key.ApiKey == dto.ApiKey);

        if (exist)
            throw new InvalidOperationException("An api Clinet with same name or with same apiKey exist");

        else
        {
            apiClient.Name = dto.Name;
            apiClient.ApiKey =dto.ApiKey; 
            apiClient.IsActive = dto.IsActive; 
            apiClient.CreatedAt = dto.CreatedAt;

            await context.SaveChangesAsync(); 

            return apiClient;

        }


    }

}
