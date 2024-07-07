using MediatR;
using OrderService.Domain.Entities;

namespace WarehouseService.Api.WarehouseService.Application.Requests
{
    public class UpdateStockRequest : IRequest<int>
    {
        public Order Order { get; set; }
    }
}
