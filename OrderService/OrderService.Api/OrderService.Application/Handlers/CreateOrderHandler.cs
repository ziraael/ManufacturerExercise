using MassTransit;
using MediatR;
using OrderService.Api.OrderService.Application.Requests;

namespace OrderService.Api.OrderService.Application.Handlers
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderRequest, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private ISendEndpointProvider _sendEndpointProvider;
        public CreateOrderHandler(IOrderRepository orderRepository, ISendEndpointProvider sendEndpointProvider)
        {
            _orderRepository = orderRepository;
            _sendEndpointProvider = sendEndpointProvider;
        }

        public async Task<bool> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            // First create the order
            var order = _orderRepository.CreateOrder(request.Order);

            //inform warehouse service
            if (order)
            {
                var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("rabbitmq://localhost/order-created-queue"));

                await endpoint.Send(request.Order);
            }
            return order;
        }
    }
}