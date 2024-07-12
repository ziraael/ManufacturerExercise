using EngineService.Api.EngineService.Application.Requests;
using MediatR;

namespace WarehouseService.Api.WarehouseService.Application.Handlers
{
    public class GetEngineProductionStatusHandler : IRequestHandler<GetEngineProductionStatusRequest, bool>
    {
        private readonly IEngineRepository _engineRepository;

        public GetEngineProductionStatusHandler(IEngineRepository engineRepository)
        {
            _engineRepository = engineRepository;
        }

        public async Task<bool> Handle(GetEngineProductionStatusRequest request, CancellationToken cancellationToken)
        {
            var order = await _engineRepository.GetEngineProductionStatus(request.OrderId);
            return order;
        }
    }
}
