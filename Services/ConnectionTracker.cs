using System.Collections.Concurrent;

namespace SignalREmitter.Services;

public class ConnectionInfo
{
    public string ConnectionId { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public DateTime ConnectedAt { get; set; }
}

public class ConnectionTracker
{
    private readonly ConcurrentDictionary<string, ConnectionInfo> _connections = new();
    private readonly ILogger<ConnectionTracker> _logger;

    public ConnectionTracker(ILogger<ConnectionTracker> logger)
    {
        _logger = logger;
    }

    public bool HasConnections => _connections.Count > 0;
    public int ConnectionCount => _connections.Count;

    public void AddConnection(string connectionId, string ipAddress)
    {
        var connectionInfo = new ConnectionInfo
        {
            ConnectionId = connectionId,
            IpAddress = ipAddress,
            ConnectedAt = DateTime.UtcNow
        };
        
        _connections.TryAdd(connectionId, connectionInfo);
        _logger.LogInformation("Connection added: {ConnectionId} from {IpAddress}. Total connections: {Count}", 
            connectionId, ipAddress, _connections.Count);
    }

    public void RemoveConnection(string connectionId)
    {
        if (_connections.TryRemove(connectionId, out var connectionInfo))
        {
            _logger.LogInformation("Connection removed: {ConnectionId} from {IpAddress}. Total connections: {Count}", 
                connectionId, connectionInfo.IpAddress, _connections.Count);
        }
    }

    public IEnumerable<ConnectionInfo> GetActiveConnections()
    {
        return _connections.Values.ToList();
    }
}
