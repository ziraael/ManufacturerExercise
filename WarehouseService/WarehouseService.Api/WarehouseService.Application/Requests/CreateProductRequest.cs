using MediatR;
using WarehouseService.Domain.Entities;

namespace WarehouseService.Api.WarehouseService.Application.Requests
{
    public class CreateProductRequest : IRequest<int>
    {
        public Product Product { get; set; }
    }
}
