using Microsoft.AspNetCore.Mvc;
using AssemblyService.Domain.Entities;

namespace AssemblyService.Api.Controllers
{
    public class AssemblyController : Controller
    {
        //private readonly IAssemblyService _assemblyService;

        public AssemblyController(/*IAssemblyService assemblyService*/)
        {
            //_assemblyService = assemblyService;
        }

        [HttpPost(nameof(AssembleOrder))]
        public IActionResult AssembleOrder([FromBody] Order order)
        {
            //assemble and deliver to warehouse

            //_assemblyService.AssembleOrder(order);
            return Ok();
        }
    }
}
