using EngineService.Domain.Entities;
using MediatR;

namespace EngineService.Api.EngineService.Application.Requests
{
    public class CreateAssembleRequest : IRequest<Engine>
    {
        public Engine Engine { get; set; }
    }
}
