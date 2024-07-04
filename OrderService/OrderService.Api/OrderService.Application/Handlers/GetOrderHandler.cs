using MediatR;
using OrderService.Api.OrderService.Application.Requests;
using OrderService.Domain.Entities;

namespace OrderService.Api.OrderService.Application.Handlers
{
    public class GetOrderHandler : IRequestHandler<GetOrderRequest, Order?>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order?> Handle(GetOrderRequest request, CancellationToken cancellationToken)
        {
            return await _orderRepository.GetOrder(request.OrderId);
        }
    }
}