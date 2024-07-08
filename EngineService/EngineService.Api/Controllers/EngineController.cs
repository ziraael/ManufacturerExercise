using OrderService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EngineService.Api.Controllers
{
    public class EngineController : Controller
    {
        //private readonly IEngineService _engineService;

        public EngineController(/*IEngineService engineService*/)
        {
            //_engineService = engineService;
        }

        [HttpPost(nameof(ProduceEngine))]
        public IActionResult ProduceEngine([FromBody] Order order)
        {
            //produce engine and send it for assembly

            //_engineService.ProduceEngine(order);
            return Ok();
        }
    }
}
