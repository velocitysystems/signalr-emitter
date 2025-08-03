using System.Collections.Concurrent;

namespace SignalREmitter.Services;

public class ConnectionTracker
{
    private readonly ConcurrentDictionary<string, DateTime> _connections = new();
    private readonly ILogger<ConnectionTracker> _logger;

    public ConnectionTracker(ILogger<ConnectionTracker> logger)
    {
        _logger = logger;
    }

    public bool HasConnections => _connections.Count > 0;
    public int ConnectionCount => _connections.Count;

    public void AddConnection(string connectionId)
    {
        _connections.TryAdd(connectionId, DateTime.UtcNow);
        _logger.LogInformation("Connection added: {ConnectionId}. Total connections: {Count}", 
            connectionId, _connections.Count);
    }

    public void RemoveConnection(string connectionId)
    {
        _connections.TryRemove(connectionId, out _);
        _logger.LogInformation("Connection removed: {ConnectionId}. Total connections: {Count}", 
            connectionId, _connections.Count);
    }

    public IEnumerable<string> GetActiveConnections()
    {
        return _connections.Keys.ToList();
    }
}
