using MassTransit;
using Microsoft.EntityFrameworkCore;
using OptionPackService.Domain;
using OptionPackService.Infrastructure;
using OptionPackService.Infrastructure.Repositories;
using OptionPackService.Api.Configurations;
using WarehouseService.Api;

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
                     ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

builder.Services.AddScoped<IOptionPackRepository, OptionPackRepository>();
builder.Services.RegisterRequestHandlers();
builder.Services.AddMassTransit(busConfig =>
{
    busConfig.AddConsumer<OrderToOptionPackConsumer>();

    busConfig.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host("localhost", "/", h =>
        {
            h.Username(builder.Configuration["MessageBroker:Username"]);
            h.Password(builder.Configuration["MessageBroker:Password"]);
        });

        configurator.ReceiveEndpoint("produce-optionpack-queue", c =>
        {
            c.ConfigureConsumer<OrderToOptionPackConsumer>(context);
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
