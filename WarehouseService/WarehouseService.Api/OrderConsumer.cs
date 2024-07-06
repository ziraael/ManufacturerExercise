using MassTransit;
using WarehouseService.Domain.Entities;

namespace WarehouseService.Api
{
    public class OrderConsumer : IConsumer<Order>
    {
        public async Task Consume(ConsumeContext<Order> context)
        {
            var msg = context.Message;
            //await Console.Out.WriteLineAsync(msg.ChassisColor);
        }
    }
}
