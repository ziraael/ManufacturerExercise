using MediatR;
using WarehouseService.Domain.Entities;

namespace WarehouseService.Api.WarehouseService.Application.Requests
{
    public class CreateWarehouseRequest : IRequest<int>
    {
        public Warehouse Warehouse { get; set; }
    }
}
