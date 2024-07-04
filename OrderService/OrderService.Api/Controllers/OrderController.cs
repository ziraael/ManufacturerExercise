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

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(GetOrderAsync))]
        public async Task<Order?> GetOrderAsync(int orderId)
        {
            var orderDetails = await _mediator.Send(new GetOrderRequest() { OrderId = orderId});

            return orderDetails;
        }

        [HttpPost(nameof(CreateOrderAsync))]
        public async Task<Order> CreateOrderAsync(Order order)
        {
            var orderId = await _mediator.Send(new CreateOrderRequest() { Order = order});
            return orderId;
        }
    }
}