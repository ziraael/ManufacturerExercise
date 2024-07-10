using EngineService.Api.EngineService.Application.Requests;
using MassTransit;
using MediatR;
using OrderService.Domain.Entities;

namespace OptionPackService.Api.Consumers
{
    public class OrderToOptionPackConsumer : IConsumer<Order>
    {
        private readonly ILogger<OrderToOptionPackConsumer> _logger;
        private IMediator _mediator;
        private ISendEndpointProvider _sendEndpointProvider;
        public OrderToOptionPackConsumer(ILogger<OrderToOptionPackConsumer> logger, IMediator mediator, ISendEndpointProvider sendEndpointProvider)
        {
            _logger = logger;
            _mediator = mediator;
            _sendEndpointProvider = sendEndpointProvider;
        }
        public async Task Consume(ConsumeContext<Order> context)
        {
            _logger.LogInformation("Hey i need an option pack produced for this specific order: ", context.Message);

            //create option pack
            var optionPack = await _mediator.Send(new CreateProductRequest() { Order = context.Message });

            //now go add to stock and send for assemble
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("rabbitmq://localhost/update-optionstock-queue"));

            await endpoint.Send(optionPack);
        }
    }
}
