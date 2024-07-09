using EngineService.Api.EngineService.Application.Requests;
using MassTransit;
using MediatR;
using OptionPackService.Domain.Entities;

namespace WarehouseService.Api.WarehouseService.Application.Handlers
{
    public class CreateProductHandler : IRequestHandler<CreateProductRequest, OptionPack>
    {
        //Inject Validators 
        private readonly IOptionPackRepository _optionPackRepository;
        private IPublishEndpoint _publishEndpoint;
        public CreateProductHandler(IOptionPackRepository optionPackRepository, IPublishEndpoint publishEndpoint)
        {
            _optionPackRepository = optionPackRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<OptionPack> Handle(CreateProductRequest request, CancellationToken cancellationToken)
        {
            var order = await _optionPackRepository.CreateOptionPack(request.Order);
            return order;
        }
    }
}
