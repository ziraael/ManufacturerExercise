using MediatR;
using Microsoft.AspNetCore.Mvc;
using OptionPackService.Api.OptionPackService.Application.Requests;

namespace OptionPackService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OptionPackController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OptionPackController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(GetOptionPackProductionStatus))]
        public async Task<bool> GetOptionPackProductionStatus(string id)
        {
            Guid orderId = new Guid(id);
            return await _mediator.Send(new GetOptionPackProductionStatusRequest() { OrderId = orderId });
        }
    }
}
