using EngineService.Api.EngineService.Application.Requests;
using MassTransit;
using MediatR;
using OrderService.Domain.Entities;

namespace EngineService.Api
{
    public class OrderToEngineConsumer : IConsumer<Order>
    {
        private readonly ILogger<OrderToEngineConsumer> _logger;
        private IMediator _mediator;
        private ISendEndpointProvider _sendEndpointProvider;
        public OrderToEngineConsumer(ILogger<OrderToEngineConsumer> logger, IMediator mediator, ISendEndpointProvider sendEndpointProvider)
        {
            _logger = logger;
            _mediator = mediator;
            _sendEndpointProvider = sendEndpointProvider;
        }
        public async Task Consume(ConsumeContext<Order> context)
        {
            _logger.LogInformation("Hey i need an engine produced for this specific order: ", context.Message);

            //create engine
            var engine = await _mediator.Send(new CreateProductRequest() { Order = context.Message });

            //now go assemble
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("rabbitmq://localhost/assembly-service-queue"));

            await endpoint.Send(engine);
        }
    }
}
