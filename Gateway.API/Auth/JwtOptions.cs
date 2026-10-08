namespace Gateway.API.Auth;

public class JwtOptions
{
    public string Key { get; set; } = "";
    public int ExpiryMinutes { get; set; }
}
