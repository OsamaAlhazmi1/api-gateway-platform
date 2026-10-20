namespace Gateway.API.Models;

public class ApiClient
{

    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string ApiKeyHash { get; set; } = "";

    public int RateLimitPerMinute { get; set; } = 10;
    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
