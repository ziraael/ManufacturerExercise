using MassTransit;
using Microsoft.EntityFrameworkCore;
using OptionPackService.Domain;
using OptionPackService.Infrastructure;
using OptionPackService.Infrastructure.Repositories;
using OptionPackService.Api.Configurations;
using OptionPackService.Api.Consumers;

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
var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "optionpackservicedb";
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

builder.Services.AddScoped<IOptionPackRepository, OptionPackRepository>();
builder.Services.RegisterRequestHandlers();
builder.Services.AddMassTransit(busConfig =>
{
    busConfig.AddConsumer<OrderToOptionPackConsumer>();

    busConfig.UsingRabbitMq((context, configurator) =>
    {
        //host.docker.internal
        configurator.Host("host.docker.internal", "/", h =>
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

app.UseCors("CorsPolicy");
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();

app.MapControllers();

app.Run();
