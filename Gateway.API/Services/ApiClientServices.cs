using Gateway.API.Data;
using Gateway.API.DTOs.ApiDefinitionDTOs;
using Gateway.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Gateway.API.Services;

public class ApiClientServices(GatewayDbContext context)
{



    public async Task<List<ApiClientResponseDTO>> GetAllAsync()
    {
        var apiList = await context.ApiClients.Select(api => new ApiClientResponseDTO(
        Id: api.Id,
        Name: api.Name,
        ApiKey: api.ApiKey,
        IsActive: api.IsActive,
        CreatedAt: api.CreatedAt

    )).AsNoTracking().ToListAsync();

        return apiList;
    }

    public async Task<ApiClientResponseDTO?> GetByIdAsync(int id)
    {
        var client = await context.ApiClients.FirstOrDefaultAsync(api => api.Id == id);

        if (client == null)
            return null;
        else
        {
            var response = new ApiClientResponseDTO(
                Id: client.Id,
                Name: client.Name,
                ApiKey: client.ApiKey,
                IsActive: client.IsActive,
                CreatedAt: client.CreatedAt
            );

            return response;
        }

    }

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
            apiClient.ApiKey = dto.ApiKey;
            apiClient.IsActive = dto.IsActive;
            apiClient.CreatedAt = dto.CreatedAt;

            await context.SaveChangesAsync();

            return apiClient;

        }

    }
    public async Task<bool> DeleteAsync(int id)
    {
        var client = await context.ApiClients.FirstOrDefaultAsync(api => api.Id == id);

        if (client == null)
            return false;
        else
        {
            context.ApiClients.Remove(client);
            await context.SaveChangesAsync();
            return true;
        }
    }

    public async Task<ApiClient?> GetByApiKeyAsync(string apiKey)
    {
        var client = await context.ApiClients.FirstOrDefaultAsync(api => api.ApiKey == apiKey);

        if (client == null)
            return null;


        return client;

    }

}
