using MediatR;
using OrderService.Domain.Entities;
using WarehouseService.Domain.Entities;

namespace WarehouseService.Api.WarehouseService.Application.Requests
{
    public class ReadOnlyStockRequest : IRequest<bool>
    {
        public Guid ProductId { get; set; }
    }
}
