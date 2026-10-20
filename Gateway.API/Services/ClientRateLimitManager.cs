using System.Collections.Concurrent;
using System.Threading.RateLimiting;

namespace Gateway.API.Services;

public class ClientRateLimitManager
{
    private readonly ConcurrentDictionary<int, FixedWindowRateLimiter>
        _limiters = new();

    private static FixedWindowRateLimiter CreateLimiter(int permitLimit)
    {
        var options = new FixedWindowRateLimiterOptions
        {
            PermitLimit = permitLimit,
            Window = TimeSpan.FromMinutes(1),
            QueueLimit = 0,
            AutoReplenishment = true
        };
        var fixedWindowRateLimiter = new FixedWindowRateLimiter(options);

        return fixedWindowRateLimiter;
    }

    public FixedWindowRateLimiter GetOrCreateLimiter(int clientId, int permitLimit)
    {
        permitLimit = Math.Max(1, permitLimit);

        var limiter = _limiters.GetOrAdd(clientId, _ => CreateLimiter(permitLimit));
        return limiter;
    }
    public void UpdateLimiter(int clientId, int permitLimit)
    {
        permitLimit = Math.Max(1, permitLimit);

        var newLimiter = CreateLimiter(permitLimit);

        _limiters.AddOrUpdate(
            clientId,
            newLimiter,
            (_, oldLimiter) =>
            {
                oldLimiter.Dispose();
                return newLimiter;
            });
    }
}
