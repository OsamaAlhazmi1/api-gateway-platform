using Gateway.API.Data;
using Gateway.API.DTOs.ApiRequestLogDTOs;
using Gateway.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Gateway.API.Services;

public class ApiRequestLogService(GatewayDbContext context)
{

    public async Task SaveAsync(ApiRequestLog log)
    {
        await context.ApiRequestLogs.AddAsync(log);
        await context.SaveChangesAsync();
    }

    public async Task<ApiRequestLog> CreateAsync(CreateApiRequestLogDTO dto)
    {
        var log = new ApiRequestLog()
        {
            ApiClientId = dto.ClinetId,
            Method = dto.Method,
            Path = dto.Path,
            StatusCode = dto.StatusCode,
            DurationMs = dto.DurationMs,
            TimestampUtc = DateTime.Now
        };
        await SaveAsync(log);
        return log;
    }

    public async Task<List<ApiRequestLogResponseDTO>> GetAllAsync()
    {
        var logList = await context.ApiRequestLogs.Select(log => new ApiRequestLogResponseDTO(

            ClinetId: log.ApiClientId,
            Method: log.Method,
            Path: log.Path,
            DurationMs: log.DurationMs,
            StatusCode: log.StatusCode,
            TimestampUtc: log.TimestampUtc

        )).AsNoTracking().ToListAsync();

        return logList;
    }

}
