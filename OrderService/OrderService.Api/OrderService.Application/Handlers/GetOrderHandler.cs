using MassTransit;
using MediatR;
using OrderService.Api.OrderService.Application.Requests;
using OrderService.Domain.Entities;

namespace OrderService.Api.OrderService.Application.Handlers
{
    public class GetOrderHandler : IRequestHandler<GetOrderRequest, Order?>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public GetOrderHandler(IOrderRepository orderRepository, IPublishEndpoint publishEndpoint)
        {
            _orderRepository = orderRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Order?> Handle(GetOrderRequest request, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(new OrderCreatedEvent
            {
                Id = new Guid(),
                CreatedAt = new DateTime()
            }, cancellationToken);
            var order = await _orderRepository.GetOrder(request.OrderId);
            return order;
        }
    }
}