using OrderService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ChassisService.Api.Controllers
{
    public class ChassisController : Controller
    {
        //private readonly IChassisService _chassisService;

        public ChassisController(/*IChassisService chassisService*/)
        {
            //_chassisService = chassisService;
        }

        [HttpPost(nameof(ProduceChassis))]
        public IActionResult ProduceChassis([FromBody] Order order)
        {
            //produce chassis and send for assembly

            //_chassisService.ProduceChassis(order);
            return Ok();
        }
    }
}
