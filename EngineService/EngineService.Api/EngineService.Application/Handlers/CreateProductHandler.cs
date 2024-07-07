using EngineService.Api.EngineService.Application.Requests;
using EngineService.Domain.Entities;
using MassTransit;
using MediatR;

namespace WarehouseService.Api.WarehouseService.Application.Handlers
{
    public class CreateProductHandler : IRequestHandler<CreateProductRequest, Engine>
    {
        //Inject Validators 
        private readonly IEngineRepository _engineRepository;
        private IPublishEndpoint _publishEndpoint;
        public CreateProductHandler(IEngineRepository engineRepository, IPublishEndpoint publishEndpoint)
        {
            _engineRepository = engineRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Engine> Handle(CreateProductRequest request, CancellationToken cancellationToken)
        {
            var order = await _engineRepository.CreateEngine(request.Order);
            return order;
        }
    }
}
