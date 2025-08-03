# Use the official .NET 8 runtime as base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the .NET 8 SDK for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SignalREmitter.csproj", "."]
RUN dotnet restore "SignalREmitter.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "SignalREmitter.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "SignalREmitter.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage - runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SignalREmitter.dll"]
