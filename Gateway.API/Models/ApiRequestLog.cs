namespace Gateway.API.Models;

public class ApiRequestLog
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Path { get; set; } = "";
    public int ApiClientId { get; set; }
    public string Method { get; set; } = "";
    public long DurationMs { get; set; }
    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;



}
