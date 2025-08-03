namespace SignalREmitter.Models;

public class BroadcastMessage
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string Type { get; set; } = "broadcast";
    public int Counter { get; set; }
    public string Status { get; set; } = "active";
}

public class SystemInfo
{
    public string ServerName { get; set; } = Environment.MachineName;
    public string Version { get; set; } = "1.0.0";
    public DateTime StartTime { get; set; }
    public int TotalMessagesSent { get; set; }
}
