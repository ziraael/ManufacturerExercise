using EngineService.Api.EngineService.Application.Requests;
using EngineService.Domain.Entities;
using MassTransit;
using MediatR;

namespace EngineService.Api.EngineService.Application.Handlers
{
    public class CreateAssembleHandler : IRequestHandler<CreateAssembleRequest, Engine>
    {
        //Inject Validators 
        private readonly IEngineRepository _engineRepository;
        private IPublishEndpoint _publishEndpoint;
        public CreateAssembleHandler(IEngineRepository engineRepository, IPublishEndpoint publishEndpoint)
        {
            _engineRepository = engineRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Engine> Handle(CreateAssembleRequest request, CancellationToken cancellationToken)
        {
            var order = await _engineRepository.CreateEngine(request.Order);
            return order;
        }
    }
}
