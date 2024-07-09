using MediatR;
using OptionPackService.Domain.Entities;
using OrderService.Domain.Entities;

namespace EngineService.Api.EngineService.Application.Requests
{
    public class CreateProductRequest : IRequest<OptionPack>
    {
        public Order Order { get; set; }
    }
}
