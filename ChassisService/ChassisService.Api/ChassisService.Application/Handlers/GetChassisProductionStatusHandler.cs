using ChassisService.Api.ChassisService.Application.Requests;
using MediatR;

namespace ChassisService.Api.ChassisService.Application.Handlers
{
    public class GetChassisProductionStatusHandler : IRequestHandler<GetChassisProductionStatusRequest, bool>
    {
        private readonly IChassisRepository _chassisRepository;

        public GetChassisProductionStatusHandler(IChassisRepository chassisRepository)
        {
            _chassisRepository = chassisRepository;
        }

        public async Task<bool> Handle(GetChassisProductionStatusRequest request, CancellationToken cancellationToken)
        {
            var order = await _chassisRepository.GetChassisProductionStatus(request.OrderId);
            return order;
        }
    }
}
