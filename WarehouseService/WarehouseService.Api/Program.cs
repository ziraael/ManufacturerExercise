using MassTransit;
using Microsoft.EntityFrameworkCore;
using WarehouseService.Api;
using WarehouseService.Api.Configurations;
using WarehouseService.Api.Consumers;
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

// Register ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                     ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

builder.Services.AddScoped<IWarehouseRepository, WarehouseRepository>();
builder.Services.RegisterRequestHandlers();
builder.Services.AddMassTransit(busConfig =>
{
    busConfig.AddConsumer<OrderConsumer>();
    busConfig.AddConsumer<StockConsumer>();
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
        
        configurator.ReceiveEndpoint("update-stock-queue", c =>
        {
            c.ConfigureConsumer<StockConsumer>(context);
        });

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
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
