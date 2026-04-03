# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Build image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files and restore as distinct layers
COPY ["DeviceManager.API/DeviceManager.API.csproj", "DeviceManager.API/"]
COPY ["DeviceManager.Core/DeviceManager.Core.csproj", "DeviceManager.Core/"]
COPY ["DeviceManager.Infrastructure/DeviceManager.Infrastructure.csproj", "DeviceManager.Infrastructure/"]
RUN dotnet restore "DeviceManager.API/DeviceManager.API.csproj"

# Copy the rest of the code and build
COPY . .
WORKDIR "/src/DeviceManager.API"
RUN dotnet build "DeviceManager.API.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "DeviceManager.API.csproj" -c Release -o /app/publish

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DeviceManager.API.dll"]
