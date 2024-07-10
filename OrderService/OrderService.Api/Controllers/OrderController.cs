using MassTransit;
using MassTransit.Transports;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Api.OrderService.Application.Requests;
using OrderService.Domain.Entities;

namespace OrderService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ISendEndpointProvider _sendEndpointProvider;


        public OrderController(IMediator mediator, IPublishEndpoint publishEndpoint, ISendEndpointProvider sendEndpointProvider)
        {
            _mediator = mediator;
            _publishEndpoint = publishEndpoint;
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpGet(nameof(GetOrder))]
        public async Task<Order?> GetOrder(int orderId)
        {
            await _publishEndpoint.Publish(new OrderCreatedEvent
            {
                Id = new Guid(),
                CreatedAt = new DateTime()
            });
            var orderDetails = await _mediator.Send(new GetOrderRequest() { OrderId = orderId });
            
            return orderDetails;
        }

        [HttpPost(nameof(CreateOrder))]
        public async Task<bool> CreateOrder(Order order)
        {
            return await _mediator.Send(new CreateOrderRequest() { Order = order });
        }

        [HttpPost(nameof(ChangeOrderStatus))]
        public async Task<bool> ChangeOrderStatus(Guid orderId, string type, bool statusValue)
        {
            return await _mediator.Send(new ChangeOrderStatusRequest() { OrderId = orderId, Type = type, StatusValue = statusValue });
        }
    }
}