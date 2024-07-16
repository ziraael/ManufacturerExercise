using EngineService.Domain.Entities;
using MassTransit;
using MassTransit.Transports;
using MediatR;
using WarehouseService.Api.WarehouseService.Application.Requests;

namespace WarehouseService.Api
{
    public class EngineStockConsumer : IConsumer<Engine>
    {
        private readonly ILogger<EngineStockConsumer> _logger;
        private IMediator _mediator;

        public EngineStockConsumer(ILogger<EngineStockConsumer> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        public async Task Consume(ConsumeContext<Engine> context)
        {
            _logger.LogInformation("Hey i produced engine, add it to stock: ", context.Message);

             await _mediator.Send(new AddToStockRequest() { Engine = context.Message });
        }
    }
}
