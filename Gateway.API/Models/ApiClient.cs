namespace Gateway.API.Models;

public class ApiClient
{

    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string ApiKey { get; set; } = "";

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
