FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Use the official .NET Core SDK image as a build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the .csproj file and restore dependencies
COPY ["EngineService/EngineService.Api/EngineService.Api.csproj", "EngineService/EngineService.Api/"]
COPY ["EngineService/EngineService.Domain/EngineService.Domain.csproj", "EngineService/EngineService.Domain/"]
COPY ["EngineService/EngineService.Infrastructure/EngineService.Infrastructure.csproj", "EngineService/EngineService.Infrastructure/"]
RUN dotnet restore "EngineService/EngineService.Api/EngineService.Api.csproj"

# Copy the remaining source files and build the application
COPY . .
WORKDIR "/src/EngineService/EngineService.Api"
RUN dotnet build "EngineService.Api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "EngineService.Api.csproj" -c Release -o /app/publish

# Use the official .NET Core runtime image as a base
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EngineService.Api.dll"]