using ChassisService.Api.Hubs;
using ChassisService.Domain.Entities;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace ChassisService.Api.Consumers
{
    public class InformFrontConsumer : IConsumer<Chassis>
    {
        private readonly ILogger<InformFrontConsumer> _logger;
        private IHubContext<ChassisHub> _hubContext;

        public InformFrontConsumer(ILogger<InformFrontConsumer> logger, IHubContext<ChassisHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }
        public async Task Consume(ConsumeContext<Chassis> context)
        {
            _logger.LogInformation("Hey i need to inform from for this specific chassis: ", context.Message);

            await _hubContext.Clients.All.SendAsync("ChassisReady", context.Message);
        }
    }
}
