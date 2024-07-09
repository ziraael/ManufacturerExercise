using MassTransit;
using MediatR;
using WarehouseService.Api.WarehouseService.Application.Requests;

namespace WarehouseService.Api.WarehouseService.Application.Handlers
{
    public class AddToStockHandler : IRequestHandler<AddToStockRequest, int>
    {
        //Inject Validators 
        private readonly IWarehouseRepository _warehouseRepository;
        private IPublishEndpoint _publishEndpoint;
        public AddToStockHandler(IWarehouseRepository warehouseRepository, IPublishEndpoint publishEndpoint)
        {
            _warehouseRepository = warehouseRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<int> Handle(AddToStockRequest request, CancellationToken cancellationToken)
        {
            return await _warehouseRepository.AddProductToStock(request.Engine, request.Chassis, request.OptionPack);
        }
    }
}
