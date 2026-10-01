using Gateway.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Gateway.API.Data;

public class GatewayDbContext(DbContextOptions<GatewayDbContext> options)
    : DbContext(options)
{
    public DbSet<ApiDefinition> ApiDefinitions => Set<ApiDefinition>();
}
