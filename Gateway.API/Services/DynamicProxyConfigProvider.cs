using Gateway.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Configuration;

namespace Gateway.API.Services;

public class DynamicProxyConfigProvider(IServiceScopeFactory scopeFactory) : IProxyConfigProvider
{
    private DynamicProxyConfig? currentConfig;
    public IProxyConfig GetConfig()
    {

        using var socpe = scopeFactory.CreateScope();
        var context = socpe.ServiceProvider.GetRequiredService<GatewayDbContext>();
        var apiDefinitions = context.ApiDefinitions.AsNoTracking().ToList();


        var routes = apiDefinitions.Select(api => new RouteConfig
        {
            RouteId = $"{api.Id}",
            ClusterId = $"{api.Id}",

            Match = new RouteMatch
            {
                Path = $"{api.RoutePrefix}/{{**catch-all}}"
            },
            Transforms =
            [
            new Dictionary<string, string>
            {
                ["PathPattern"] = $"{api.DownstreamPath}/{{**catch-all}}"
            }
            ]

        }).ToList();

        var clusters = apiDefinitions.Select(api => new ClusterConfig
        {

            ClusterId = $"{api.Id}",

            Destinations = new Dictionary<string, DestinationConfig>
            {
                [$"destination-{api.Id}"] = new DestinationConfig
                {
                    Address = api.DestinationAddress
                }
            }

        }).ToList();
        currentConfig = new DynamicProxyConfig(routes, clusters);


        return currentConfig;
    }


    public void Reload()
    {
        currentConfig?.SignalChange();
    }

}
