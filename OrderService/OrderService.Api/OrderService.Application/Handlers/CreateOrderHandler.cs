using MediatR;
using OrderService.Api.OrderService.Application.Requests;
using OrderService.Domain.Entities;

namespace OrderService.Api.OrderService.Application.Handlers
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderRequest, Order>
    {
        //Inject Validators 
        private readonly IOrderRepository _orderRepository;

        public CreateOrderHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            // First validate the request
            return await _orderRepository.CreateOrder(request.Order);
        }
    }
}