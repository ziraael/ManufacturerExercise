using MediatR;

namespace EngineService.Api.EngineService.Application.Requests
{
    public class GetEngineProductionStatusRequest : IRequest<bool>
    {
        public Guid OrderId { get; set; }
    }
}
