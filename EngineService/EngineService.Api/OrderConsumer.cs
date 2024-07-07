using EngineService.Api.EngineService.Application.Requests;
using MassTransit;
using MediatR;
using OrderService.Domain.Entities;

namespace EngineService.Api
{
    public class OrderConsumer : IConsumer<Order>
    {
        private readonly ILogger<OrderConsumer> _logger;
        private IMediator _mediator;
        public OrderConsumer(ILogger<OrderConsumer> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        public async Task Consume(ConsumeContext<Order> context)
        {
            _logger.LogInformation("Hey i need an engine produced for this specific order: ", context.Message);

            //create engine
            var engine = await _mediator.Send(new CreateProductRequest() { Order = context.Message });

            //now go assemble
            await _mediator.Send(new CreateAssembleRequest() { Engine = engine });
        }
    }
}
