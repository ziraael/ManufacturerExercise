using MediatR;
using OptionPackService.Api.OptionPackService.Application.Requests;

namespace OptionPackService.Api.OptionPackService.Application.Handlers
{
    public class GetOptionPackProductionStatusHandler : IRequestHandler<GetOptionPackProductionStatusRequest, bool>
    {
        private readonly IOptionPackRepository _optionPackRepository;

        public GetOptionPackProductionStatusHandler(IOptionPackRepository optionPackRepository)
        {
            _optionPackRepository = optionPackRepository;
        }

        public async Task<bool> Handle(GetOptionPackProductionStatusRequest request, CancellationToken cancellationToken)
        {
            var order = await _optionPackRepository.GetOptionPackProductionStatus(request.OrderId);
            return order;
        }
    }
}
