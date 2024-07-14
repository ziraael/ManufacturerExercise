using EngineService.Api.Hubs;
using EngineService.Domain.Entities;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace EngineService.Api.Consumers
{
    public class InformFrontConsumer : IConsumer<Engine>
    {
        private readonly ILogger<InformFrontConsumer> _logger;
        private IHubContext<EngineHub> _hubContext;

        public InformFrontConsumer(ILogger<InformFrontConsumer> logger, IHubContext<EngineHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }
        public async Task Consume(ConsumeContext<Engine> context)
        {
            _logger.LogInformation("Hey i need to inform from for this specific engine: ", context.Message);

            await _hubContext.Clients.All.SendAsync("EngineReady", context.Message);
        }
    }
}
