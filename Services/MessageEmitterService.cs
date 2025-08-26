using Microsoft.AspNetCore.SignalR;
using SignalREmitter.Hubs;
using SignalREmitter.Models;

namespace SignalREmitter.Services;

public class MessageEmitterService : BackgroundService
{
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IHubContext<SecureChatHub> _secureHubContext;
    private readonly ILogger<MessageEmitterService> _logger;
    private readonly SystemInfo _systemInfo;
    private readonly ConnectionTracker _connectionTracker;
    private int _messageCounter = 0;

    private readonly string[] _sampleMessages = {
        "To be, or not to be, that is the question.",
        "All the world's a stage, and all the men and women merely players.",
        "Romeo, Romeo, wherefore art thou Romeo?",
        "What's in a name? That which we call a rose by any other name would smell as sweet.",
        "The course of true love never did run smooth.",
        "All that glisters is not gold.",
        "Brevity is the soul of wit.",
        "To thine own self be true.",
        "The lady doth protest too much, methinks.",
        "Better three hours too soon than a minute too late.",
        "Now is the winter of our discontent.",
        "We know what we are, but know not what we may be."
    };

    public MessageEmitterService(IHubContext<ChatHub> hubContext, IHubContext<SecureChatHub> secureHubContext, ILogger<MessageEmitterService> logger, ConnectionTracker connectionTracker)
    {
        _hubContext = hubContext;
        _secureHubContext = secureHubContext;
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

        var message = new Message
        {
            Content = _sampleMessages[(_messageCounter - 1) % _sampleMessages.Length],
            Timestamp = DateTime.UtcNow
        };

        await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
        await _secureHubContext.Clients.All.SendAsync("ReceiveMessage", message);
        
        _logger.LogInformation("Broadcasted message #{counter}: {message}", 
            _messageCounter, message.Content);
    }
}
