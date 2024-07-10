using ChassisService.Api.ChassisService.Application.Requests;
using MassTransit;
using MediatR;
using OrderService.Domain.Entities;

namespace ChassisService.Api.Consumers
{
    public class OrderToChassisConsumer : IConsumer<Order>
    {
        private readonly ILogger<OrderToChassisConsumer> _logger;
        private IMediator _mediator;
        private ISendEndpointProvider _sendEndpointProvider;
        public OrderToChassisConsumer(ILogger<OrderToChassisConsumer> logger, IMediator mediator, ISendEndpointProvider sendEndpointProvider)
        {
            _logger = logger;
            _mediator = mediator;
            _sendEndpointProvider = sendEndpointProvider;
        }
        public async Task Consume(ConsumeContext<Order> context)
        {
            _logger.LogInformation("Hey i need a chassis produced for this specific order: ", context.Message);

            //create chassis
            var chassis = await _mediator.Send(new CreateProductRequest() { Order = context.Message });

            //now go add to stock and send for assemble
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("rabbitmq://localhost/update-chassisstock-queue"));

            await endpoint.Send(chassis);
        }
    }
}
