using Microsoft.AspNetCore.SignalR;
using SignalREmitter.Hubs;
using SignalREmitter.Models;

namespace SignalREmitter.Services;

public class MessageEmitterService : BackgroundService
{
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly ILogger<MessageEmitterService> _logger;
    private readonly SystemInfo _systemInfo;
    private readonly ConnectionTracker _connectionTracker;
    private int _messageCounter = 0;

    private readonly string[] _sampleMessages = {
        "Hello from SignalR!",
        "Testing mobile integration",
        "Real-time message broadcasting",
        "SignalR is working perfectly",
        "Mobile clients should receive this",
        "Cross-platform messaging test",
        "Live data streaming active",
        "Connection status: healthy"
    };

    public MessageEmitterService(IHubContext<ChatHub> hubContext, ILogger<MessageEmitterService> logger, ConnectionTracker connectionTracker)
    {
        _hubContext = hubContext;
        _logger = logger;
        _connectionTracker = connectionTracker;
        _systemInfo = new SystemInfo
        {
            StartTime = DateTime.UtcNow
        };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("MessageEmitterService started at: {time}", DateTimeOffset.Now);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (_connectionTracker.HasConnections)
                {
                    await BroadcastMessage();
                }
                else
                {
                    _logger.LogDebug("No active connections, skipping broadcast");
                }
                
                await Task.Delay(5000, stoppingToken); // 5 seconds
            }
            catch (OperationCanceledException)
            {
                // Expected when cancellation is requested
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while broadcasting message");
                await Task.Delay(1000, stoppingToken); // Wait 1 second before retry
            }
        }

        _logger.LogInformation("MessageEmitterService stopped at: {time}", DateTimeOffset.Now);
    }

    private async Task BroadcastMessage()
    {
        _messageCounter++;
        _systemInfo.TotalMessagesSent = _messageCounter;

        var message = new BroadcastMessage
        {
            Message = _sampleMessages[(_messageCounter - 1) % _sampleMessages.Length],
            Counter = _messageCounter,
            Timestamp = DateTime.UtcNow
        };

        await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
        
        _logger.LogInformation("Broadcasted message #{counter}: {message}", 
            _messageCounter, message.Message);
    }
}
