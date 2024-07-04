using MediatR;
using OrderService.Domain.Entities;

namespace OrderService.Api.OrderService.Application.Requests
{
    public class GetOrderRequest : IRequest<Order?>
    {
        public int OrderId { get; set; }
    }
}