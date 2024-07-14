using MediatR;
using WarehouseService.Domain.Entities;

namespace WarehouseService.Api.WarehouseService.Application.Requests
{
    public class GetProductsRequest : IRequest<List<Product>>
    {

    }
}