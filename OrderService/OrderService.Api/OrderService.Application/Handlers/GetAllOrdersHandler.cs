using MassTransit;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using OrderService.Api.Hubs;
using OrderService.Api.OrderService.Application.Requests;
using OrderService.Domain.Entities;

namespace OrderService.Api.OrderService.Application.Handlers
{
    public class GetAllOrdersHandler : IRequestHandler<GetAllOrdersRequest, List<Order>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IHubContext<OrderHub> _hubContext;

        public GetAllOrdersHandler(IOrderRepository orderRepository, IHubContext<OrderHub> hubContext)
        {
            _orderRepository = orderRepository;
            _hubContext = hubContext;
        }

        public async Task<List<Order>> Handle(GetAllOrdersRequest request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAllOrders();
            return order;
        }
    }
}