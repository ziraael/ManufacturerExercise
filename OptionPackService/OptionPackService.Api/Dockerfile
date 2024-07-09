FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Use the official .NET Core SDK image as a build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the .csproj file and restore dependencies
COPY ["OptionPackService/OptionPackService.Api/OptionPackService.Api.csproj", "OptionPackService/OptionPackService.Api/"]
COPY ["OptionPackService/OptionPackService.Domain/OptionPackService.Domain.csproj", "OptionPackService/OptionPackService.Domain/"]
COPY ["OptionPackService/OptionPackService.Infrastructure/OptionPackService.Infrastructure.csproj", "OptionPackService/OptionPackService.Infrastructure/"]
RUN dotnet restore "OptionPackService/OptionPackService.Api/OptionPackService.Api.csproj"

# Copy the remaining source files and build the application
COPY . .
WORKDIR "/src/OptionPackService/OptionPackService.Api"
RUN dotnet build "OptionPackService.Api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "OptionPackService.Api.csproj" -c Release -o /app/publish

# Use the official .NET Core runtime image as a base
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OptionPackService.Api.dll"]