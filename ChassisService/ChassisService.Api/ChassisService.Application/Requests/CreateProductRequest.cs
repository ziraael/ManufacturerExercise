using ChassisService.Domain.Entities;
using MediatR;
using OrderService.Domain.Entities;

namespace ChassisService.Api.ChassisService.Application.Requests
{
    public class CreateProductRequest : IRequest<Chassis>
    {
        public Order Order { get; set; }
    }
}
