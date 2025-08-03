using Microsoft.AspNetCore.SignalR;
using SignalREmitter.Services;

namespace SignalREmitter.Hubs;

public class ChatHub : Hub
{
    private readonly ConnectionTracker _connectionTracker;

    public ChatHub(ConnectionTracker connectionTracker)
    {
        _connectionTracker = connectionTracker;
    }
    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).SendAsync("UserJoined", $"User {Context.ConnectionId} joined {groupName}");
    }

    public async Task LeaveGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).SendAsync("UserLeft", $"User {Context.ConnectionId} left {groupName}");
    }

    public override async Task OnConnectedAsync()
    {
        var ipAddress = Context.GetHttpContext()?.Connection?.RemoteIpAddress?.ToString() ?? "Unknown";
        _connectionTracker.AddConnection(Context.ConnectionId, ipAddress);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _connectionTracker.RemoveConnection(Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
}
