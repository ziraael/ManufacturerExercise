using MediatR;

namespace ChassisService.Api.ChassisService.Application.Requests
{
    public class GetChassisProductionStatusRequest : IRequest<bool>
    {
        public Guid OrderId { get; set; }
    }
}
