using SignalREmitter.Hubs;
using SignalREmitter.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddSignalR();
builder.Services.AddSingleton<ConnectionTracker>();
builder.Services.AddHostedService<MessageEmitterService>();

// Add CORS for mobile client testing
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseCors();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api", () => "SignalR Emitter is running!");
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }));
app.MapGet("/api/connections", (ConnectionTracker connectionTracker) => 
{
    var connections = connectionTracker.GetActiveConnections().Select(c => new 
    {
        connectionId = c.ConnectionId,
        ipAddress = c.IpAddress,
        connectedAt = c.ConnectedAt,
        duration = DateTime.UtcNow - c.ConnectedAt
    });
    
    return Results.Ok(new 
    {
        count = connectionTracker.ConnectionCount,
        connections = connections
    });
});


app.MapHub<ChatHub>("/chatHub");

app.Run();
