using Gateway.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Gateway.API.Data;

public class GatewayDbContext(DbContextOptions<GatewayDbContext> options)
    : DbContext(options)
{
    public DbSet<ApiDefinition> ApiDefinitions => Set<ApiDefinition>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApiDefinition>().HasData(
            new ApiDefinition
            {
                Id = 1,
                Name = "Users API",
                RoutePrefix = "/users",
                DestinationAddress = "http://localhost:5024/"
            },
            new ApiDefinition
            {
                Id = 2,
                Name = "Orders API",
                RoutePrefix = "/orders",
                DestinationAddress = "http://localhost:5297/"
            }
        );
    }



}

