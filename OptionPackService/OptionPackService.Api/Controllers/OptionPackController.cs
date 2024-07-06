using Microsoft.AspNetCore.Mvc;
using OptionPackService.Domain.Entities;

namespace OptionPackService.Api.Controllers
{
    public class OptionPackController : Controller
    {
        //private readonly IOptionPackService _optionPackService;

        public OptionPackController(/*IOptionPackService optionPackService*/)
        {
            //_optionPackService = optionPackService;
        }

        [HttpPost(nameof(ProduceOptionPack))]
        public IActionResult ProduceOptionPack([FromBody] Order order)
        {
            //produce option pack and send for assembly

            //_optionPackService.ProduceOptionPack(order);
            return Ok();
        }
    }
}
