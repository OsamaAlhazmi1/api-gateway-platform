using System.Security.Cryptography;
using System.Text;
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
        ApiKey: api.ApiKeyHash,
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
                ApiKey: client.ApiKeyHash,
                IsActive: client.IsActive,
                CreatedAt: client.CreatedAt
            );

            return response;
        }

    }

    public async Task<(ApiClient Client, string Apikey)> CreateAsync(string name)
    {
        var apikey = Convert.ToBase64String(
        RandomNumberGenerator.GetBytes(32));

        var client = new ApiClient()
        {
            Name = name,
            ApiKeyHash = HashAPIKey(apikey),
            IsActive = true
        };
        await context.ApiClients.AddAsync(client);
        await context.SaveChangesAsync();

        return (client, apikey);
    }

    public async Task<ApiClient?> GetByApiKeyAsync(string apiKey)
    {
        var hashKey = HashAPIKey(apiKey);
        var client = await context.ApiClients.FirstOrDefaultAsync(api => api.ApiKeyHash == hashKey && api.IsActive == true);

        if (client == null)
            return null;

        return client;

    }

    public async Task<ApiClient?> UpdateAsync(int id, UpdateApiClientDTO dto)
    {
        var apiClient = await context.ApiClients.FirstOrDefaultAsync(c => c.Id == id);

        if (apiClient == null)
            return null;
        var hashedDTOApiKey = HashAPIKey(dto.ApiKey);

        var exist = await context.ApiClients.AnyAsync(
        key => key.Name == dto.Name &&
        key.Id == id
        || key.ApiKeyHash == hashedDTOApiKey);

        if (exist)
            throw new InvalidOperationException("An api Clinet with same name or with same apiKey exist");

        else
        {
            apiClient.Name = dto.Name;
            apiClient.ApiKeyHash = hashedDTOApiKey;
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


    private static string HashAPIKey(string apikey)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(apikey));
        return Convert.ToBase64String(hash);
    }


    public async Task<bool> DeactivateApiKeyAsync(int id)
    {
        var client = await context.ApiClients
            .FirstOrDefaultAsync(client => client.Id == id);

        if (client is null || !client.IsActive)
            return false;

        client.IsActive = false;

        await context.SaveChangesAsync();

        return true;
    }
    public async Task<bool> ActivateApiKeyAsync(int id)
    {
        var client = await context.ApiClients
            .FirstOrDefaultAsync(client => client.Id == id);

        if (client is null || client.IsActive)
            return false;

        client.IsActive = true;

        await context.SaveChangesAsync();

        return true;
    }


}
