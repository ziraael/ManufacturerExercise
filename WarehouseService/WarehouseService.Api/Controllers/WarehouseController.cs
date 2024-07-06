using Microsoft.AspNetCore.Mvc;
using WarehouseService.Domain.Entities;

namespace WarehouseService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WarehouseController : ControllerBase
    {
        //private readonly IWarehouseService _warehouseService;

        public WarehouseController(/*IWarehouseService warehouseService*/)
        {
            //_warehouseService = warehouseService;
        }

        [HttpPost(nameof(CheckStock))]
        public IActionResult CheckStock([FromBody] Order order)
        {
            //var hasStock = _warehouseService.CheckStock(order);
            return Ok();
        }

        [HttpPost(nameof(UpdateStock))]
        public IActionResult UpdateStock([FromBody] Order order)
        {
            //_warehouseService.UpdateStock(order);
            return Ok();
        }

        public bool IsBeforeCollection(Guid orderId)
        {
            var res = false;
            //check is before collection
            return res;
        }
    }

}
