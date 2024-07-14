using MassTransit;
using MediatR;
using WarehouseService.Api.WarehouseService.Application.Requests;
using WarehouseService.Domain.Entities;

namespace WarehouseService.Api.WarehouseService.Application.Handlers
{
    public class GetProductsHandler : IRequestHandler<GetProductsRequest, List<Product>>
    {
        //Inject Validators 
        private readonly IWarehouseRepository _warehouseRepository;
        public GetProductsHandler(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        public async Task<List<Product>> Handle(GetProductsRequest request, CancellationToken cancellationToken)
        {
            return await _warehouseRepository.GetProducts();
        }
    }
}
