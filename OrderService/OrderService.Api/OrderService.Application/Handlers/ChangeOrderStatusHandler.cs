using MediatR;
using OrderService.Api.OrderService.Application.Requests;

namespace OrderService.Api.OrderService.Application.Handlers
{
    public class ChangeOrderStatusHandler : IRequestHandler<ChangeOrderStatusRequest, bool>
    {
        //Inject Validators 
        private readonly IOrderRepository _orderRepository;
        public ChangeOrderStatusHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<bool> Handle(ChangeOrderStatusRequest request, CancellationToken cancellationToken)
        {
            return _orderRepository.ChangeOrderStatus(request.OrderId, request.Type, request.StatusValue);
        }
    }
}
