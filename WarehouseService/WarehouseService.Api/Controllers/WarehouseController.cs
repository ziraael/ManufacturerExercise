using MediatR;
using Microsoft.AspNetCore.Mvc;
using WarehouseService.Api.WarehouseService.Application.Requests;
using WarehouseService.Domain.Entities;
using OrderService.Domain.Entities;

namespace WarehouseService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WarehouseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WarehouseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(nameof(CheckStock))]
        public async Task<bool> CheckStock([FromBody] Order order)
        {
            return await _mediator.Send(new CheckStockRequest() { Order = order });
        }

        [HttpPost(nameof(CreateProduct))]
        public async Task<bool> CreateProduct(Product product)
        {
            var res = await _mediator.Send(new CreateProductRequest() { Product = product });

            if(res > 0)
            {
                return true;
            }

            return false;
        }

        [HttpPost(nameof(CreateWarehouse))]
        public async Task<bool> CreateWarehouse(Warehouse warehouse)
        {
            var res = await _mediator.Send(new CreateWarehouseRequest() { Warehouse = warehouse });

            if (res > 0)
            {
                return true;
            }

            return false;
        }
    }

}
