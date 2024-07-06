using MediatR;
using OrderService.Domain.Entities;

namespace OrderService.Api.OrderService.Application.Requests
{
    public class CreateOrderRequest : IRequest<bool>
    {
        public Order Order { get; set; }
    }
}