using MediatR;
using OptionPackService.Api.OptionPackService.Application.Requests;
using OptionPackService.Domain.Entities;

namespace OptionPackService.Api.OptionPackService.Application.Handlers
{
    public class CreateProductHandler : IRequestHandler<CreateProductRequest, OptionPack>
    {
        private readonly IOptionPackRepository _optionPackRepository;
        public CreateProductHandler(IOptionPackRepository optionPackRepository)
        {
            _optionPackRepository = optionPackRepository;
        }

        public async Task<OptionPack> Handle(CreateProductRequest request, CancellationToken cancellationToken)
        {
            var order = await _optionPackRepository.CreateOptionPack(request.Order);
            return order;
        }
    }
}
