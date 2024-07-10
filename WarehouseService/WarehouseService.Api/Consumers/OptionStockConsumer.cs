using MassTransit;
using MediatR;
using OptionPackService.Domain.Entities;
using WarehouseService.Api.WarehouseService.Application.Requests;

namespace WarehouseService.Api
{
    public class OptionStockConsumer : IConsumer<OptionPack>
    {
        private readonly ILogger<OptionStockConsumer> _logger;
        private IMediator _mediator;
        private ISendEndpointProvider _sendEndpointProvider;
        public OptionStockConsumer(ILogger<OptionStockConsumer> logger, IMediator mediator, ISendEndpointProvider sendEndpointProvider)
        {
            _logger = logger;
            _mediator = mediator;
            _sendEndpointProvider = sendEndpointProvider;
        }
        public async Task Consume(ConsumeContext<OptionPack> context)
        {
            _logger.LogInformation("Hey i produced option pack, add it to stock: ", context.Message);

            await _mediator.Send(new AddToStockRequest() { OptionPack = context.Message });
        }
    }
}
