using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using OrderService.Domain.Entities;
using WarehouseService.Api.WarehouseService.Application.Requests;

namespace WarehouseService.Api
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
            _logger.LogInformation("Hey order created: ", context.Message);

            //check if there is stock first

            //lets update stock
            var res = await _mediator.Send(new UpdateStockRequest() { Order = context.Message });

            //stock updated?
        }
    }
}
