using MediatR;
using OrderService.Api.OrderService.Application.Requests;
using OrderService.Domain.Entities;

namespace OrderService.Api.OrderService.Application.Handlers
{
    public class GetAllOrdersHandler : IRequestHandler<GetAllOrdersRequest, List<Order>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetAllOrdersHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<List<Order>> Handle(GetAllOrdersRequest request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAllOrders();
            return order;
        }
    }
}