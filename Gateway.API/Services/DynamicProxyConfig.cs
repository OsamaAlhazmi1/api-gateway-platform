using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Configuration;

namespace Gateway.API.Services;

public class DynamicProxyConfig(
    IReadOnlyList<RouteConfig> routes,
    IReadOnlyList<ClusterConfig> clusters): IProxyConfig
{
    public IReadOnlyList<RouteConfig> Routes { get; } = routes;

    public IReadOnlyList<ClusterConfig> Clusters { get; } = clusters;

    public IChangeToken ChangeToken => new CancellationChangeToken(
        CancellationToken.None);
}
