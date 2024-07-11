using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.Api.Configurations;
using OrderService.Api.Consumers;
using OrderService.Domain;
using OrderService.Infrastructure;
using OrderService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var serviceProvider = builder.Services.BuildServiceProvider();
var logger = serviceProvider.GetService<ILogger<ApplicationLogger>>();
builder.Services.AddSingleton(typeof(ILogger), logger);

// Register ApplicationDbContext
var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "orderservicedb";
var dbPassword = Environment.GetEnvironmentVariable("DB_ROOT_PASSWORD") ?? "root";
var connectionString = $"Server={dbHost};Port=3306;Database={dbName};User Id=root;Password={dbPassword};";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(connectionString), ServiceLifetime.Transient);

// Add services to the container.
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.RegisterRequestHandlers();
builder.Services.AddMassTransit(busConfig =>
{
    busConfig.AddConsumer<OrderConsumer>();
    //busConfig.SetKebabCaseEndpointNameFormatter();
    busConfig.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host("host.docker.internal", "/", y =>
        {
            y.Username(builder.Configuration["MessageBroker:Username"]!);
            y.Username(builder.Configuration["MessageBroker:Password"]!);
        });

        configurator.ReceiveEndpoint("ready-collection-queue", c =>
        {
            c.ConfigureConsumer<OrderConsumer>(context);
        });
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(builder => builder
     .AllowAnyOrigin()
     .AllowAnyMethod()
     .AllowAnyHeader());
app.UseAuthorization();

app.MapControllers();

app.Run();
