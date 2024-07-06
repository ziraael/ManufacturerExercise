using MassTransit;
using MediatR;
using OrderService.Api.OrderService.Application.Requests;
using OrderService.Domain.Entities;

namespace OrderService.Api.OrderService.Application.Handlers
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderRequest, Order>
    {
        //Inject Validators 
        private readonly IOrderRepository _orderRepository;
        private IPublishEndpoint _publishEndpoint;
        public CreateOrderHandler(IOrderRepository orderRepository, IPublishEndpoint publishEndpoint)
        {
            _orderRepository = orderRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Order> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            // First validate the request
            await _publishEndpoint.Publish(new OrderCreatedEvent{
                Id = new Guid(),
                CreatedAt = new DateTime()
            },cancellationToken);
            var order = await _orderRepository.CreateOrder(request.Order);
            return order;
        }
    }
}