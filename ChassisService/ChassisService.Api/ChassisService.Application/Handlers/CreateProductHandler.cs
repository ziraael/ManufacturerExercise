using ChassisService.Api.ChassisService.Application.Requests;
using ChassisService.Domain.Entities;
using MediatR;

namespace ChassisService.Api.WarehouseService.Application.Handlers
{
    public class CreateProductHandler : IRequestHandler<CreateProductRequest, Chassis>
    {
        private readonly IChassisRepository _chassisRepository;
        public CreateProductHandler(IChassisRepository chassisRepository)
        {
            _chassisRepository = chassisRepository;
        }

        public async Task<Chassis> Handle(CreateProductRequest request, CancellationToken cancellationToken)
        {
            var order = await _chassisRepository.CreateChassis(request.Order);
            return order;
        }
    }
}
