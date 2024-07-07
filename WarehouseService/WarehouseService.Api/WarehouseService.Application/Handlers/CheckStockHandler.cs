using MassTransit;
using MediatR;
using OrderService.Domain.Entities;
using WarehouseService.Api.WarehouseService.Application.Requests;

namespace WarehouseService.Api.WarehouseService.Application.Handlers
{
    public class CheckStockHandler : IRequestHandler<CheckStockRequest, bool>
    {
        //Inject Validators 
        private readonly IWarehouseRepository _warehouseRepository;
        private IPublishEndpoint _publishEndpoint;
        public CheckStockHandler(IWarehouseRepository warehouseRepository, IPublishEndpoint publishEndpoint)
        {
            _warehouseRepository = warehouseRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<bool> Handle(CheckStockRequest request, CancellationToken cancellationToken)
        {
            // First create the order
            var hasStock = _warehouseRepository.CheckStock(request.Order);

            //inform
            //await _publishEndpoint.Publish(hasStock);
            return hasStock;
        }
    }
}