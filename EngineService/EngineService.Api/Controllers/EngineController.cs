using Microsoft.AspNetCore.Mvc;
using MediatR;
using EngineService.Api.EngineService.Application.Requests;

namespace EngineService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EngineController : ControllerBase
    {
        private readonly IMediator _mediator;
        public EngineController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(GetEngineProductionStatus))]
        public async Task<bool> GetEngineProductionStatus(string id)
        {
            Guid orderId = new Guid(id);
            return await _mediator.Send(new GetEngineProductionStatusRequest() { OrderId = orderId });
        }
    }
}
