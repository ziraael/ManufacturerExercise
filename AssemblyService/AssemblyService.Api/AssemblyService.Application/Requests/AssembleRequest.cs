using EngineService.Domain.Entities;
using MediatR;
using OrderService.Domain.Entities;

namespace EngineService.Api.EngineService.Application.Requests
{
    public class AssembleRequest : IRequest<Engine>
    {
        public Order Order { get; set; }
    }
}
