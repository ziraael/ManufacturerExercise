using MediatR;
using OrderService.Domain.Entities;

namespace OrderService.Api.OrderService.Application.Requests
{
    public class GetOrderRequest : IRequest<Order?>
    {
        public Guid OrderId { get; set; }
    }
}