using MassTransit;
using MassTransit.Transports;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Api.OrderService.Application.Requests;
using OrderService.Domain.DTOs;
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

        [HttpGet(nameof(GetOrders))]
        public async Task<List<Order>> GetOrders()
        {
            return await _mediator.Send(new GetAllOrdersRequest() { });
        }

        [HttpGet(nameof(GetOrderById))]
        public async Task<Order?> GetOrderById(Guid orderId)
        {
            return await _mediator.Send(new GetOrderRequest() { OrderId = orderId });
        }

        [HttpPost(nameof(CreateOrder))]
        public async Task<bool> CreateOrder(Order order)
        {
            return await _mediator.Send(new CreateOrderRequest() { Order = order });
        }

        [HttpPost(nameof(ChangeOrderStatus))]
        public async Task<bool> ChangeOrderStatus(ChangeOrderStatusDTO request)
        {
            Guid id = new Guid(request.OrderId);
            return await _mediator.Send(new ChangeOrderStatusRequest() { OrderId = id, Type = request.Type, StatusValue = request.StatusValue });
        }
    }
}