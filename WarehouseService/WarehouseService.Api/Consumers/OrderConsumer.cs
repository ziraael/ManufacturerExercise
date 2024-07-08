using MassTransit;
using MassTransit.Transports;
using MediatR;
using Microsoft.Extensions.Logging;
using OrderService.Domain.Entities;
using WarehouseService.Api.WarehouseService.Application.Requests;

namespace WarehouseService.Api.Consumers
{
    public class OrderConsumer : IConsumer<Order>
    {
        private readonly ILogger<OrderConsumer> _logger;
        private IMediator _mediator;
        private ISendEndpointProvider _sendEndpointProvider;
        public OrderConsumer(ILogger<OrderConsumer> logger, IMediator mediator, ISendEndpointProvider sendEndpointProvider)
        {
            _logger = logger;
            _mediator = mediator;
            _sendEndpointProvider = sendEndpointProvider;
        }
        public async Task Consume(ConsumeContext<Order> context)
        {
            _logger.LogInformation("Hey order created: ", context.Message);

            //check if there is stock first in assembled vehicles
            var hasAssembledVehicleAvailable = await _mediator.Send(new CheckAssembledVehiclesStockRequest() { Order = context.Message });

            //if not, check if there are seperate components available, if not start producing
            if (!hasAssembledVehicleAvailable)
            {
                await _mediator.Send(new CheckStockRequest() { Order = context.Message });
            }
            //ready for collection, inform order service
            else
            {
                var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("rabbitmq://localhost/ready-collection-queue"));

                await endpoint.Send(context.Message);
            }
        }
    }
}
