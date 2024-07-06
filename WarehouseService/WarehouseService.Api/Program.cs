using MassTransit;
using WarehouseService.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(busConfig =>
{
    busConfig.AddConsumer<OrderConsumer>();

    // Explicitly configure the receive endpoint (queue)
    busConfig.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(new Uri(builder.Configuration["MessageBroker:Host"]!), y =>
        {
            y.Username(builder.Configuration["MessageBroker:Username"]!);
            y.Password(builder.Configuration["MessageBroker:Password"]!);
        });

        // Point the receive endpoint to the same queue name used in OrderService
        configurator.ReceiveEndpoint(
            builder.Configuration["MessageBroker:Queue"], // Ensure queue name matches OrderService
            ep =>
            {
                ep.ConfigureConsumer<OrderConsumer>(context);
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
