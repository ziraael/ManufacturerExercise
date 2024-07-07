using MediatR;
using OrderService.Api.OrderService.Application.Requests;

namespace OrderService.Api.OrderService.Application.Handlers
{
    public class CancelOrderHandler : IRequestHandler<CancelOrderRequest, bool>
    {
        //Inject Validators 
        private readonly IOrderRepository _orderRepository;
        public CancelOrderHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<bool> Handle(CancelOrderRequest request, CancellationToken cancellationToken)
        {
            return _orderRepository.CancelOrder(request.OrderId);
        }
    }
}
