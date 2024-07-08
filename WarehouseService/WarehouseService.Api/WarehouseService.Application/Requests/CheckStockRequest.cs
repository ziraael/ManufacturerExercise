using MediatR;
using OrderService.Domain.Entities;

namespace WarehouseService.Api.WarehouseService.Application.Requests
{
    public class CheckStockRequest : IRequest<bool>
    {
        public Order Order { get; set; }
    }
}
