using MediatR;
using WarehouseService.Domain.DTOs;

namespace WarehouseService.Api.WarehouseService.Application.Requests
{
    public class CreateAssembledVehicleRequest : IRequest<int>
    {
        public StockDTO Stock { get; set; }
    }
}
