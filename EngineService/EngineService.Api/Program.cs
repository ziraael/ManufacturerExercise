using EngineService.Api.Configurations;
using EngineService.Domain;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using EngineService.Infrastructure;
using EngineService.Infrastructure.Repositories;
using EngineService.Api.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.UseInlineDefinitionsForEnums();
});

var serviceProvider = builder.Services.BuildServiceProvider();
var logger = serviceProvider.GetService<ILogger<ApplicationLogger>>();
builder.Services.AddSingleton(typeof(ILogger), logger);

// Register ApplicationDbContext
var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "engineservicedb";
var dbPassword = Environment.GetEnvironmentVariable("DB_ROOT_PASSWORD") ?? "root";
var connectionString = $"Server={dbHost};Port=3306;Database={dbName};User Id=root;Password={dbPassword};";

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .WithOrigins("http://localhost:4200") // Angular app URL
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(connectionString), ServiceLifetime.Transient);

builder.Services.AddScoped<IEngineRepository, EngineRepository>();
builder.Services.RegisterRequestHandlers();
builder.Services.AddMassTransit(busConfig =>
{
    busConfig.AddConsumer<OrderToEngineConsumer>();

    busConfig.UsingRabbitMq((context, configurator) =>
    {
        bool IsRunningInContainer = bool.TryParse(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), out var inDocker) && inDocker;
        var host = IsRunningInContainer ? "host.docker.internal" : "localhost";

        configurator.Host(host, "/", h =>
        {
            h.Username(builder.Configuration["MessageBroker:Username"]);
            h.Password(builder.Configuration["MessageBroker:Password"]);
        });

        configurator.ReceiveEndpoint("produce-engine-queue", c =>
        {
            c.ConfigureConsumer<OrderToEngineConsumer>(context);
        });
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

app.UseCors("CorsPolicy");
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
