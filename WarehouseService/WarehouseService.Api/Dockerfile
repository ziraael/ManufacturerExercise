FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Use the official .NET Core SDK image as a build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the .csproj file and restore dependencies
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

#ENV ConnectionString="Server=localhost;Port=3303;Database=warehouseservicedb;User Id=root;Password=root;"

# Use the official .NET Core runtime image as a base
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Run migrations
#ENV PATH $PATH:/root/.dotnet/tools
#RUN dotnet tool install -g dotnet-ef
#RUN dotnet ef database update --project build/WarehouseService.Api.csproj
ENTRYPOINT ["dotnet", "WarehouseService.Api.dll"]
