using MediatR;
using OrderService.Domain.Entities;

namespace OrderService.Api.OrderService.Application.Requests
{
    public class GetAllOrdersRequest : IRequest<List<Order>>
    {
    }
}