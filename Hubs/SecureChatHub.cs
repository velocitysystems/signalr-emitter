using SignalREmitter.Services;

namespace SignalREmitter.Hubs;

public class SecureChatHub : ChatHub
{
    public SecureChatHub(ConnectionTracker connectionTracker) : base(connectionTracker)
    {
    }
}
