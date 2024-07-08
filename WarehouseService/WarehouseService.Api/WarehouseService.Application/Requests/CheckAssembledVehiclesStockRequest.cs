using MediatR;
using OrderService.Domain.Entities;

namespace WarehouseService.Api.WarehouseService.Application.Requests
{
    public class CheckAssembledVehiclesStockRequest : IRequest<bool>
    {
        public Order Order { get; set; }
    }
}
