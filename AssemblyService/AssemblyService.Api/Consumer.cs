using AssemblyService.Domain.Entities;
using EngineService.Api.EngineService.Application.Requests;
using MassTransit;
using MediatR;

namespace AssemblyService.Api
{
    public class Consumer : IConsumer<Engine>
    {
        private readonly ILogger<Consumer> _logger;
        private IMediator _mediator;
        private ISendEndpointProvider _sendEndpointProvider;
        public Consumer(ILogger<Consumer> logger, IMediator mediator, ISendEndpointProvider sendEndpointProvider)
        {
            _logger = logger;
            _mediator = mediator;
            _sendEndpointProvider = sendEndpointProvider;
        }
        public async Task Consume(ConsumeContext<Engine> context)
        {
            _logger.LogInformation("Hey i need to assemble this for this order: ", context.Message);

            //assemble
            var engine = await _mediator.Send(new AssembleEngineRequest() { Engine = context.Message });

            //now inform warehouse i have finished assembling
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("rabbitmq://localhost/assembly-complete-queue"));

            await endpoint.Send(engine.order);
        }
    }
}
