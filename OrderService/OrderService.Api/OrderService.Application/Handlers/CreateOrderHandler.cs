using MassTransit;
using MediatR;
using OrderService.Api.OrderService.Application.Requests;

namespace OrderService.Api.OrderService.Application.Handlers
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderRequest, bool>
    {
        //Inject Validators 
        private readonly IOrderRepository _orderRepository;
        private IPublishEndpoint _publishEndpoint;
        public CreateOrderHandler(IOrderRepository orderRepository, IPublishEndpoint publishEndpoint)
        {
            _orderRepository = orderRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<bool> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            // First create the order
            var order = _orderRepository.CreateOrder(request.Order);

            //inform
            await _publishEndpoint.Publish(request.Order);
            return order;
        }
    }
}