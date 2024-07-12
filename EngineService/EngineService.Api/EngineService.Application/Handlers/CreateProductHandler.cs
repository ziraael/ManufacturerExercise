using EngineService.Api.EngineService.Application.Requests;
using EngineService.Domain.Entities;
using MediatR;

namespace EngineService.Api.EngineService.Application.Handlers
{
    public class CreateProductHandler : IRequestHandler<CreateProductRequest, Engine>
    {
        private readonly IEngineRepository _engineRepository;
        public CreateProductHandler(IEngineRepository engineRepository)
        {
            _engineRepository = engineRepository;
        }

        public async Task<Engine> Handle(CreateProductRequest request, CancellationToken cancellationToken)
        {
            var order = await _engineRepository.CreateEngine(request.Order);
            return order;
        }
    }
}
