using ChassisService.Domain.Entities;
using MassTransit;
using MediatR;
using WarehouseService.Api.WarehouseService.Application.Requests;

namespace WarehouseService.Api
{
    public class ChassisStockConsumer : IConsumer<Chassis>
    {
        private readonly ILogger<ChassisStockConsumer> _logger;
        private IMediator _mediator;
        private ISendEndpointProvider _sendEndpointProvider;
        public ChassisStockConsumer(ILogger<ChassisStockConsumer> logger, IMediator mediator, ISendEndpointProvider sendEndpointProvider)
        {
            _logger = logger;
            _mediator = mediator;
            _sendEndpointProvider = sendEndpointProvider;
        }
        public async Task Consume(ConsumeContext<Chassis> context)
        {
            _logger.LogInformation("Hey i produced engine, add it to stock: ", context.Message);

            await _mediator.Send(new AddToStockRequest() { Chassis = context.Message });
        }
    }
}
