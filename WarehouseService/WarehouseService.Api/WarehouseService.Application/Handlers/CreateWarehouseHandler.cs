using MassTransit;
using MediatR;
using WarehouseService.Api.WarehouseService.Application.Requests;

namespace WarehouseService.Api.WarehouseService.Application.Handlers
{
    public class CreateWarehouseHandler : IRequestHandler<CreateWarehouseRequest, int>
    {
        //Inject Validators 
        private readonly IWarehouseRepository _warehouseRepository;
        private IPublishEndpoint _publishEndpoint;
        public CreateWarehouseHandler(IWarehouseRepository warehouseRepository, IPublishEndpoint publishEndpoint)
        {
            _warehouseRepository = warehouseRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<int> Handle(CreateWarehouseRequest request, CancellationToken cancellationToken)
        {
            var order = await _warehouseRepository.CreateWarehouse(request.Warehouse);
            return order;
        }
    }
}
