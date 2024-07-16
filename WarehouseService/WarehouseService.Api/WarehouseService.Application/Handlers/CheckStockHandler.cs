using MediatR;
using WarehouseService.Api.WarehouseService.Application.Requests;

namespace WarehouseService.Api.WarehouseService.Application.Handlers
{
    public class CheckStockHandler : IRequestHandler<CheckStockRequest, bool>
    {
        //Inject Validators 
        private readonly IWarehouseRepository _warehouseRepository;
        public CheckStockHandler(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        public async Task<bool> Handle(CheckStockRequest request, CancellationToken cancellationToken)
        {
            var order = await _warehouseRepository.CheckStock(request.Order);
            return order;
        }
    }
}
