using MediatR;

namespace OptionPackService.Api.OptionPackService.Application.Requests
{
    public class GetOptionPackProductionStatusRequest : IRequest<bool>
    {
        public Guid OrderId { get; set; }
    }
}
