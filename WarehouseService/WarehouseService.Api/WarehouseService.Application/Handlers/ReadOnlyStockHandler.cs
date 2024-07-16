using MediatR;
using WarehouseService.Api.WarehouseService.Application.Requests;
using WarehouseService.Domain.Entities;

namespace WarehouseService.Api.WarehouseService.Application.Handlers
{
    public class ReadOnlyStockHandler : IRequestHandler<ReadOnlyStockRequest, bool>
    {
        private readonly IWarehouseRepository _warehouseRepository;
        public ReadOnlyStockHandler(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        public async Task<bool> Handle(ReadOnlyStockRequest request, CancellationToken cancellationToken)
        {
            var order = await _warehouseRepository.ReadOnlyStock(request.ProductId);
            return order;
        }
    }
}
