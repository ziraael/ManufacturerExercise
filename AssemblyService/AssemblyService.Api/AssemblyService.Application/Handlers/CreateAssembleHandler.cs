using EngineService.Api.EngineService.Application.Requests;
using EngineService.Domain.Entities;
using MassTransit;
using MediatR;

namespace WarehouseService.Api.WarehouseService.Application.Handlers
{
    public class CreateAssembleHandler : IRequestHandler<AssembleRequest, Engine>
    {
        //Inject Validators 
        private readonly IEngineRepository _engineRepository;
        private IPublishEndpoint _publishEndpoint;
        public CreateAssembleHandler(IEngineRepository engineRepository, IPublishEndpoint publishEndpoint)
        {
            _engineRepository = engineRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Engine> Handle(AssembleRequest request, CancellationToken cancellationToken)
        {
            var order = await _engineRepository.CreateEngine(request.Order);
            return order;
        }
    }
}
