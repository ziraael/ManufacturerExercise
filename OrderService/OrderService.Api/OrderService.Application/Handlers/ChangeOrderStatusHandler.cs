using MassTransit;
using MediatR;
using OrderService.Api.OrderService.Application.Requests;

namespace OrderService.Api.OrderService.Application.Handlers
{
    public class ChangeOrderStatusHandler : IRequestHandler<ChangeOrderStatusRequest, bool>
    {
        //Inject Validators 
        private readonly IOrderRepository _orderRepository;
        private IPublishEndpoint _publishEndpoint;
        public ChangeOrderStatusHandler(IOrderRepository orderRepository, IPublishEndpoint publishEndpoint)
        {
            _orderRepository = orderRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<bool> Handle(ChangeOrderStatusRequest request, CancellationToken cancellationToken)
        {
            // First create the order
            var order = _orderRepository.ChangeOrderStatus(request.OrderId, request.Type, request.StatusValue);

            //inform
            //await _publishEndpoint.Publish(request.Order);
            return order;
        }
    }
}
