using MediatR;

namespace OrderService.Api.OrderService.Application.Requests
{
    public class CancelOrderRequest : IRequest<bool>
    {
        public Guid OrderId { get; set; }
    }
}
