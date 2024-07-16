using MassTransit;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using OrderService.Api.Hubs;
using OrderService.Api.OrderService.Application.Requests;
using WarehouseService.Domain.Entities;

namespace OrderService.Api.Consumers
{
    public class OrderConsumer : IConsumer<AssembledVehicleStock>
    {
        private readonly ILogger<OrderConsumer> _logger;
        private IMediator _mediator;
        private IHubContext<OrderHub> _hubContext;

        public OrderConsumer(ILogger<OrderConsumer> logger, IMediator mediator,IHubContext<OrderHub> hubContext)
        {
            _logger = logger;
            _mediator = mediator;
            _hubContext = hubContext;
        }
        public async Task Consume(ConsumeContext<AssembledVehicleStock> context)
        {
            _logger.LogInformation("Hey i have the assembled the vehicle, make it ready for collection: ", context.Message);

            if (context.Message.OrderId != null)
            {
                await _mediator.Send(new ChangeOrderStatusRequest() { OrderId = (Guid)context.Message.OrderId, Type = "IsReadyForCollection", StatusValue = true });

                await _hubContext.Clients.All.SendAsync("IsReadyForCollection", context.Message.OrderId);
            }
        }
    }
}