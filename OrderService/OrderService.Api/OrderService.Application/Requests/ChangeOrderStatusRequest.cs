using MediatR;

namespace OrderService.Api.OrderService.Application.Requests
{
    public class ChangeOrderStatusRequest : IRequest<bool>
    {
        public Guid OrderId { get; set; }
        public string Type { get; set; }
        public bool StatusValue { get; set; }

    }
}
