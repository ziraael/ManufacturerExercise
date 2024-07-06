using MassTransit;
using OrderService.Domain.Entities;
using OrderService.Domain.Entities;

namespace OrderService.Api
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