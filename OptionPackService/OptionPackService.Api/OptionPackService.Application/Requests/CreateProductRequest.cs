using MediatR;
using OptionPackService.Domain.Entities;
using OrderService.Domain.Entities;

namespace OptionPackService.Api.OptionPackService.Application.Requests
{
    public class CreateProductRequest : IRequest<OptionPack>
    {
        public Order Order { get; set; }
    }
}
