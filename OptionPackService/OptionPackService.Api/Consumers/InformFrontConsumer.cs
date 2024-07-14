using MassTransit;
using Microsoft.AspNetCore.SignalR;
using OptionPackService.Api.Hubs;
using OptionPackService.Domain.Entities;

namespace OptionPackService.Api.Consumers
{
    public class InformFrontConsumer : IConsumer<OptionPack>
    {
        private readonly ILogger<InformFrontConsumer> _logger;
        private IHubContext<OptionHub> _hubContext;

        public InformFrontConsumer(ILogger<InformFrontConsumer> logger, IHubContext<OptionHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }
        public async Task Consume(ConsumeContext<OptionPack> context)
        {
            _logger.LogInformation("Hey i need to inform from for this specific option pack: ", context.Message);

            await _hubContext.Clients.All.SendAsync("OptionReady", context.Message);
        }
    }
}
