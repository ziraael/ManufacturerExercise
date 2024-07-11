FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Use the official .NET Core SDK image as a build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the .csproj files and restore dependencies
COPY ["WarehouseService/WarehouseService.Api/WarehouseService.Api.csproj", "WarehouseService/WarehouseService.Api/"]
COPY ["WarehouseService/WarehouseService.Domain/WarehouseService.Domain.csproj", "WarehouseService/WarehouseService.Domain/"]
COPY ["WarehouseService/WarehouseService.Infrastructure/WarehouseService.Infrastructure.csproj", "WarehouseService/WarehouseService.Infrastructure/"]
RUN dotnet restore "WarehouseService/WarehouseService.Api/WarehouseService.Api.csproj"

# Copy the remaining source files and build the application 
COPY . .
WORKDIR "/src/WarehouseService/WarehouseService.Api"
RUN dotnet build "WarehouseService.Api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "WarehouseService.Api.csproj" -c Release -o /app/publish

# Use the official .NET Core runtime image as a base
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Copy the WarehouseService.Infrastructure and WarehouseService.Domain projects for EF migrations
COPY --from=build /src/WarehouseService/WarehouseService.Infrastructure /app/WarehouseService.Infrastructure
COPY --from=build /src/WarehouseService/WarehouseService.Domain /app/WarehouseService.Domain

# Copy the appsettings.json file
COPY --from=build /src/WarehouseService/WarehouseService.Api/appsettings.json /app/WarehouseService.Infrastructure

ENTRYPOINT ["dotnet", "WarehouseService.Api.dll"]