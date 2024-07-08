using ChassisService.Api.ChassisService.Application.Requests;
using ChassisService.Domain.Entities;
using MassTransit;
using MediatR;

namespace ChassisService.Api.WarehouseService.Application.Handlers
{
    public class CreateProductHandler : IRequestHandler<CreateProductRequest, Chassis>
    {
        //Inject Validators 
        private readonly IChassisRepository _chassisRepository;
        private IPublishEndpoint _publishEndpoint;
        public CreateProductHandler(IChassisRepository chassisRepository, IPublishEndpoint publishEndpoint)
        {
            _chassisRepository = chassisRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Chassis> Handle(CreateProductRequest request, CancellationToken cancellationToken)
        {
            var order = await _chassisRepository.CreateChassis(request.Order);
            return order;
        }
    }
}
