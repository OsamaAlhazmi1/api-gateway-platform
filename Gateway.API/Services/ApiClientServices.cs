using System.Security.Cryptography;
using System.Text;
using Gateway.API.Data;
using Gateway.API.DTOs.ApiClientDTOs;
using Gateway.API.DTOs.ApiDefinitionDTOs;
using Gateway.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Gateway.API.Services;

public class ApiClientServices(GatewayDbContext context,ClientRateLimitManager clientRateLimitManager)
{
    public async Task<List<ApiClientResponseDTO>> GetAllAsync()
    {
        var ClientsList = await context.ApiClients.Select(c => new ApiClientResponseDTO(
        Id: c.Id,
        Name: c.Name,
        ApiKey: c.ApiKeyHash,
        IsActive: c.IsActive,
        RateLimit:c.RateLimitPerMinute,
        CreatedAt: c.CreatedAt

    )).AsNoTracking().ToListAsync();

        return ClientsList;
    }

    public async Task<ApiClientResponseDTO?> GetByIdAsync(int id)
    {
        var client = await context.ApiClients.FirstOrDefaultAsync(c => c.Id == id);

        if (client == null)
            return null;
        else
        {
            var response = new ApiClientResponseDTO(
                Id: client.Id,
                Name: client.Name,
                ApiKey: client.ApiKeyHash,
                IsActive: client.IsActive,
                RateLimit: client.RateLimitPerMinute,
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
        var client = await context.ApiClients.FirstOrDefaultAsync(c => c.ApiKeyHash == hashKey && c.IsActive == true);

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
        var client = await context.ApiClients.FirstOrDefaultAsync(c => c.Id == id);

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
            .FirstOrDefaultAsync(c => c.Id == id);

        if (client is null || !client.IsActive)
            return false;

        client.IsActive = false;

        await context.SaveChangesAsync();

        return true;
    }
    public async Task<bool> ActivateApiKeyAsync(int id)
    {
        var client = await context.ApiClients
            .FirstOrDefaultAsync(c => c.Id == id);

        if (client is null || client.IsActive)
            return false;

        client.IsActive = true;

        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool>UpdateClientRateLimitAsync(int id ,int rateLimit)
    {
        var client = await context.ApiClients.FirstOrDefaultAsync(c=> c.Id == id); 

        if (client == null )
            return false; 
        
        client.RateLimitPerMinute = rateLimit ;
        await context.SaveChangesAsync(); 

        clientRateLimitManager.UpdateLimiter(id, rateLimit);
        
        return true; 
    }


}
