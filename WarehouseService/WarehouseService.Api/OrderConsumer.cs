using MassTransit;
using Microsoft.Extensions.Logging;
using OrderService.Domain.Entities;

namespace WarehouseService.Api
{
    public class OrderConsumer : IConsumer<Order>
    {
        private readonly ILogger<OrderConsumer> _logger;
        public OrderConsumer(ILogger<OrderConsumer> logger)
        {
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<Order> context)
        {
            _logger.LogInformation("Hey order created: ", context.Message);
            //var msg = context.Message;
            //await Console.Out.WriteLineAsync(msg.ChassisColor);
        }
    }
}
