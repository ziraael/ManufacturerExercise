using MassTransit;
using MediatR;
using WarehouseService.Api.WarehouseService.Application.Requests;
using WarehouseService.Domain.DTOs;

namespace WarehouseService.Api.Consumers
{
    public class AssembleConsumer : IConsumer<StockDTO>
    {
        private readonly ILogger<AssembleConsumer> _logger;
        private IMediator _mediator;
        private ISendEndpointProvider _sendEndpointProvider;
        public AssembleConsumer(ILogger<AssembleConsumer> logger, IMediator mediator, ISendEndpointProvider sendEndpointProvider)
        {
            _logger = logger;
            _mediator = mediator;
            _sendEndpointProvider = sendEndpointProvider;
        }
        public async Task Consume(ConsumeContext<StockDTO> context)
        {
            //_logger.LogInformation("Hey i have the parts from stock, assemble for me: ", context.Message);

            //await _mediator.Send(new CreateAssembledVehicleRequest() { Stock = context.Message });
        }
    }
}
