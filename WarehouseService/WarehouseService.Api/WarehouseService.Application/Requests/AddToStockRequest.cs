using ChassisService.Domain.Entities;
using EngineService.Domain.Entities;
using MediatR;
using OptionPackService.Domain.Entities;
using OrderService.Domain.Entities;

namespace WarehouseService.Api.WarehouseService.Application.Requests
{
    public class AddToStockRequest : IRequest<int>
    {
        public Engine? Engine { get; set; }
        public Chassis? Chassis { get; set; }
        public OptionPack? OptionPack { get; set; }
    }
}