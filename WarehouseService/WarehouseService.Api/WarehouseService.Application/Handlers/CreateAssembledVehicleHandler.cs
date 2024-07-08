using MassTransit;
using MediatR;
using WarehouseService.Api.WarehouseService.Application.Requests;

namespace WarehouseService.Api.WarehouseService.Application.Handlers
{
    public class CreateAssembledVehicleHandler : IRequestHandler<CreateAssembledVehicleRequest, int>
    {
        //Inject Validators 
        private readonly IWarehouseRepository _warehouseRepository;
        private IPublishEndpoint _publishEndpoint;
        public CreateAssembledVehicleHandler(IWarehouseRepository warehouseRepository, IPublishEndpoint publishEndpoint)
        {
            _warehouseRepository = warehouseRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<int> Handle(CreateAssembledVehicleRequest request, CancellationToken cancellationToken)
        {
            var order = await _warehouseRepository.AssembleVehicle(request.Stock);
            return order;
        }
    }
}
