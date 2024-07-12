using Microsoft.AspNetCore.Mvc;
using MediatR;
using ChassisService.Api.ChassisService.Application.Requests;

namespace ChassisService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChassisController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChassisController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(GetChassisProductionStatus))]
        public async Task<bool> GetChassisProductionStatus(string id)
        {
            Guid orderId = new Guid(id);
            return await _mediator.Send(new GetChassisProductionStatusRequest() { OrderId = orderId });
        }
    }
}
