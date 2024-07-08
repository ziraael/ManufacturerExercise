using MassTransit;
using MediatR;
using WarehouseService.Api.WarehouseService.Application.Requests;

namespace WarehouseService.Api.WarehouseService.Application.Handlers
{
    public class AddEngineToStockHandler : IRequestHandler<AddEngineToStockRequest, int>
    {
        //Inject Validators 
        private readonly IWarehouseRepository _warehouseRepository;
        private IPublishEndpoint _publishEndpoint;
        public AddEngineToStockHandler(IWarehouseRepository warehouseRepository, IPublishEndpoint publishEndpoint)
        {
            _warehouseRepository = warehouseRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<int> Handle(AddEngineToStockRequest request, CancellationToken cancellationToken)
        {
            var order = await _warehouseRepository.AddEngineToStock(request.Engine);
            return order;
        }
    }
}
