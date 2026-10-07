using Gateway.API.Data;
using Gateway.API.DTOs;
using Gateway.API.DTOs.ApiDefinitionDTOs;
using Gateway.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Gateway.API.Services;

public class ApiDefinitionService(GatewayDbContext context)
{
    public async Task<List<ApiDefinitionResponseDTO>> GetAllAsync()
    {

        var apiList = await context.ApiDefinitions.Select(api => new ApiDefinitionResponseDTO(
            Id: api.Id,
            Name: api.Name,
            RoutePrefix: api.RoutePrefix,
            DownstreamPath: api.DownstreamPath
            
        )).AsNoTracking().ToListAsync();

        return apiList;

    }

    public async Task<ApiDefinitionResponseDTO?> GetByIdAsync(int id)
    {
        var api = await context.ApiDefinitions.FirstOrDefaultAsync(p => p.Id == id);
        if (api == null)
            return null;

        var response = new ApiDefinitionResponseDTO(
            Id: api.Id,
            Name: api.Name,
            RoutePrefix: api.RoutePrefix,
            DownstreamPath: api.DownstreamPath

        );
        return response;
    }

    public async Task<ApiDefinition?> CreateAsync(CreateApiDefinitionDTO dto)
    {

        var exist = await context.ApiDefinitions.FirstOrDefaultAsync
        (
        api => api.Name == dto.Name ||
        api.RoutePrefix == dto.RoutePrefix
        );

        if (exist != null)
            return null;

        var newApi = new ApiDefinition()
        {
            Name = dto.Name,
            RoutePrefix = dto.RoutePrefix,
            DestinationAddress = dto.DestinationAddress,
            DownstreamPath =dto.DownstreamPath
            
            
        };
        await context.ApiDefinitions.AddAsync(newApi);
        await context.SaveChangesAsync();

        return newApi;
    }

    public async Task<ApiDefinition?> UpdateAsync(int id, UpdateApiDefinitionDTO dto)
    {
        var api = await context.ApiDefinitions.FirstOrDefaultAsync(p => p.Id == id);

        if (api == null)
            return null;

        var exist = await context.ApiDefinitions.AnyAsync(existing =>
        existing.Id != id &&
        (existing.Name == dto.Name ||
         existing.RoutePrefix == dto.RoutePrefix));

        if (exist)
            throw new InvalidOperationException("An API with the same name or route prefix already exists.");

        else
        {
            api.Name = dto.Name;
            api.RoutePrefix = dto.RoutePrefix;
            api.DestinationAddress = dto.DestinationAddress;
            api.DownstreamPath =dto.DownstreamPath;
            await context.SaveChangesAsync();

            return api;
        }

    }

    public async Task<bool> DeleteAsync(int id)
    {
        var api = await context.ApiDefinitions.FirstOrDefaultAsync(p => p.Id == id);
        if (api == null)
            return false;
        
        context.ApiDefinitions.Remove(api);
        await context.SaveChangesAsync();

        return true;


    }
    public async Task<GatewayDbContext> GetContextAsync()
    {
        return context;
    }
}
