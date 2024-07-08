using EngineService.Domain.Entities;
using MediatR;
using OrderService.Domain.Entities;

namespace WarehouseService.Api.WarehouseService.Application.Requests
{
    public class AddEngineToStockRequest : IRequest<int>
    {
        public Engine Engine { get; set; }
    }
}
