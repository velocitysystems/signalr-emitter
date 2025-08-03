# SignalR Emitter

A simple .NET 8 ASP.NET Core application that broadcasts messages via SignalR every 5 seconds. Perfect for testing SignalR integration with mobile applications (Android/iOS).

## Features

- **Real-time messaging**: Broadcasts dynamic messages every 5 seconds
- **Simple JSON format**: Easy to parse message structure with basic field types
- **CORS enabled**: Ready for cross-origin requests from mobile apps
- **Health endpoint**: `/health` for monitoring
- **Test client**: Built-in HTML client for testing
- **Production ready**: Configured for cloud deployment

## Quick Start

### Local Development

```bash
# Restore dependencies
dotnet restore

# Run the application
dotnet run
```

The application will start on `http://localhost:5000` (or `https://localhost:5001`).

### Testing

1. Open your browser to `http://localhost:5000`
2. Click "Connect" to establish SignalR connection
3. Watch messages appear every 5 seconds

## SignalR Endpoints

- **Hub URL**: `/chatHub`
- **Message Event**: `ReceiveMessage`
- **System Info Event**: `SystemInfo` (every 10th message)

## Message Format

```json
{
  "id": "guid-string",
  "message": "Hello from SignalR!",
  "timestamp": "2024-01-01T12:00:00.000Z",
  "type": "broadcast",
  "counter": 42,
  "status": "active"
}
```

## API Endpoints

- `GET /` - Basic status message
- `GET /health` - Health check endpoint
- `WS /chatHub` - SignalR hub connection

## Deployment

### Using dotnet publish

```bash
# Build for production
dotnet publish -c Release -o ./publish

# Upload the publish folder to your server
# Run with:
dotnet SignalREmitter.dll
```

### Using Docker

```bash
# Build Docker image
docker build -t signalr-emitter .

# Run container
docker run -p 80:80 signalr-emitter
```

## Environment Variables

The application uses standard ASP.NET Core configuration:

- `ASPNETCORE_ENVIRONMENT=Production`
- `ASPNETCORE_URLS=http://0.0.0.0:$PORT`

## Mobile Client Integration

### Connection URL
```
wss://your-domain.com/chatHub
```

### Sample Mobile Code (Conceptual)

```javascript
// JavaScript/React Native example
const connection = new signalR.HubConnectionBuilder()
    .withUrl("wss://your-domain.com/chatHub")
    .build();

connection.on("ReceiveMessage", (message) => {
    console.log("Received:", message);
});

connection.start();
```

## Troubleshooting

### Common Issues

1. **CORS errors**: The app is configured with permissive CORS for testing
2. **Connection timeouts**: Check network connectivity and firewall settings
3. **Port conflicts**: The app uses the PORT environment variable or defaults to 5000

### Logs

Check the application logs for connection events:
- Client connections/disconnections
- Message broadcast confirmations
- Error messages

## Development

### Project Structure

```
├── Hubs/
│   └── ChatHub.cs          # SignalR hub
├── Models/
│   └── MessageModels.cs    # Data models
├── Services/
│   └── MessageEmitterService.cs  # Background service
├── wwwroot/
│   └── index.html          # Test client
├── Program.cs              # Application entry point
└── SignalREmitter.csproj   # Project file
```

### Customization

- **Message interval**: Modify the delay in `MessageEmitterService.cs`
- **Message content**: Update the `_sampleMessages` array
- **Message format**: Modify the `BroadcastMessage` model

## License

This project is for testing purposes.
SignalR Emitter
