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

        [HttpGet(nameof(ReadOnlyStock))]
        public async Task<bool> ReadOnlyStock(string prodId)
        {
            Guid id = new Guid(prodId);
            return await _mediator.Send(new ReadOnlyStockRequest() { ProductId = id });
        }

        [HttpPost(nameof(CreateProduct))]
        public async Task<bool> CreateProduct([FromBody] Product product)
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

        [HttpGet(nameof(GetProducts))]
        public async Task<List<Product>> GetProducts()
        {
            return await _mediator.Send(new GetProductsRequest() { });
        }
    }

}
