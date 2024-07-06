using MassTransit;
using MediatR;
using OrderService.Api.OrderService.Application.Requests;

namespace OrderService.Api.OrderService.Application.Handlers
{
    public class CancelOrderHandler : IRequestHandler<CancelOrderRequest, bool>
    {
        //Inject Validators 
        private readonly IOrderRepository _orderRepository;
        private IPublishEndpoint _publishEndpoint;
        public CancelOrderHandler(IOrderRepository orderRepository, IPublishEndpoint publishEndpoint)
        {
            _orderRepository = orderRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<bool> Handle(CancelOrderRequest request, CancellationToken cancellationToken)
        {
            var order = _orderRepository.CancelOrder(request.OrderId);

            //inform that i canceled the order
            await _publishEndpoint.Publish(order);
            return order;
        }
    }
}
