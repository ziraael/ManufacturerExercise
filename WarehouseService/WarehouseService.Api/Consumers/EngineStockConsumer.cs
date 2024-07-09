using EngineService.Domain.Entities;
using MassTransit;
using MassTransit.Transports;
using MediatR;
using Microsoft.Extensions.Logging;
using OrderService.Domain.Entities;
using WarehouseService.Api.WarehouseService.Application.Requests;

namespace WarehouseService.Api
{
    public class EngineStockConsumer : IConsumer<Engine>
    {
        private readonly ILogger<EngineStockConsumer> _logger;
        private IMediator _mediator;
        private ISendEndpointProvider _sendEndpointProvider;
        public EngineStockConsumer(ILogger<EngineStockConsumer> logger, IMediator mediator, ISendEndpointProvider sendEndpointProvider)
        {
            _logger = logger;
            _mediator = mediator;
            _sendEndpointProvider = sendEndpointProvider;
        }
        public async Task Consume(ConsumeContext<Engine> context)
        {
            _logger.LogInformation("Hey i produced engine, add it to stock: ", context.Message);

             await _mediator.Send(new AddToStockRequest() { Engine = context.Message });
        }
    }
}
