FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Use the official .NET Core SDK image as a build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the .csproj file and restore dependencies
COPY ["ChassisService/ChassisService.Api/ChassisService.Api.csproj", "ChassisService/ChassisService.Api/"]
COPY ["ChassisService/ChassisService.Domain/ChassisService.Domain.csproj", "ChassisService/ChassisService.Domain/"]
COPY ["ChassisService/ChassisService.Infrastructure/ChassisService.Infrastructure.csproj", "ChassisService/ChassisService.Infrastructure/"]
RUN dotnet restore "ChassisService/ChassisService.Api/ChassisService.Api.csproj"

# Copy the remaining source files and build the application
COPY . .
WORKDIR "/src/ChassisService/ChassisService.Api"
RUN dotnet build "ChassisService.Api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "ChassisService.Api.csproj" -c Release -o /app/publish

# Use the official .NET Core runtime image as a base
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChassisService.Api.dll"]