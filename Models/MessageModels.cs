namespace SignalREmitter.Models;

public class Message
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Content { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public class SystemInfo
{
    public string ServerName { get; set; } = Environment.MachineName;
    public string Version { get; set; } = "1.0.0";
    public DateTime StartTime { get; set; }
    public int TotalMessagesSent { get; set; }
}
