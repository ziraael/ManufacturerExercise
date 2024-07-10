using MassTransit;
using MediatR;
using OrderService.Api.OrderService.Application.Requests;
using OrderService.Domain.Entities;
using WarehouseService.Domain.Entities;

namespace OrderService.Api.Consumers
{
    public class OrderConsumer : IConsumer<AssembledVehicleStock>
    {
        private readonly ILogger<OrderConsumer> _logger;
        private IMediator _mediator;
        public OrderConsumer(ILogger<OrderConsumer> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        public async Task Consume(ConsumeContext<AssembledVehicleStock> context)
        {
            _logger.LogInformation("Hey i have the assembled the vehicle, make it ready for collection: ", context.Message);

            if (context.Message.OrderId != null)
            {
                await _mediator.Send(new ChangeOrderStatusRequest() { OrderId = (Guid)context.Message.OrderId, Type = "IsReadyForCollection", StatusValue = true });
            }
        }
    }
}