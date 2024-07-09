using MassTransit;
using Microsoft.EntityFrameworkCore;
using WarehouseService.Api;
using WarehouseService.Api.Configurations;
using WarehouseService.Api.Consumers;
using WarehouseService.Domain;
using WarehouseService.Infrastructure;
using WarehouseService.Infrastructure.Repositories;

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
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                     ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))),ServiceLifetime.Transient);

builder.Services.AddScoped<IWarehouseRepository, WarehouseRepository>();
builder.Services.RegisterRequestHandlers();
builder.Services.AddMassTransit(busConfig =>
{
    busConfig.AddConsumer<OrderConsumer>();
    busConfig.AddConsumer<EngineStockConsumer>();
    busConfig.AddConsumer<ChassisStockConsumer>();
    busConfig.AddConsumer<AssembleConsumer>();

    busConfig.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host("localhost", "/", h =>
        {
            h.Username(builder.Configuration["MessageBroker:Username"]);
            h.Password(builder.Configuration["MessageBroker:Password"]);
        });

        configurator.ReceiveEndpoint("order-created-queue", c =>
        {
            c.ConfigureConsumer<OrderConsumer>(context);
        });
        
        configurator.ReceiveEndpoint("update-enginestock-queue", c =>
        {
            c.ConfigureConsumer<EngineStockConsumer>(context);
        });

        configurator.ReceiveEndpoint("update-chassisstock-queue", c =>
        {
            c.ConfigureConsumer<ChassisStockConsumer>(context);
        });

        //configurator.ReceiveEndpoint("update-optionstock-queue", c =>
        //{
        //    c.ConfigureConsumer<EngineStockConsumer>(context);
        //});

        configurator.ReceiveEndpoint("assemble-vehicle-queue", c =>
        {
            c.ConfigureConsumer<AssembleConsumer>(context);
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

app.UseAuthorization();

app.MapControllers();

app.Run();
