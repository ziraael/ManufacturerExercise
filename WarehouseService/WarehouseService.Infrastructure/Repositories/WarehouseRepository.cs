using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderService.Domain.Entities;
using WarehouseService.Domain.Entities;

namespace WarehouseService.Infrastructure.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly ApplicationDbContext _context;
    //private ILogger _logger;
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public WarehouseRepository(ApplicationDbContext context/*, ILogger logger*/, ISendEndpointProvider sendEndpointProvider)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        //_logger = logger;
        _sendEndpointProvider = sendEndpointProvider ?? throw new ArgumentNullException();
    }

    public bool CheckStock(Order order)
    {
        try
        {
            var warehouses = _context.Warehouses.ToList();

            if (warehouses.Any())
            {
               // var hasEngine = warehouse
            }
            //_context.Warehouses.Add(warehouse);
            _context.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "An issue occured while trying to create warehouse!");
            throw;
        }
    }

    public async Task<int> UpdateStock(Order order)
    {
        try
        {
            var engineProduct = _context.Products.FirstOrDefault(x => x.Id == order.EngineId);

            if (engineProduct != null)
            {
                //does stock exist for engine?
                var hasStock = _context.Stocks.Where(x => x.ProductId == engineProduct.Id);

                //no, produce it
                if (!hasStock.Any())
                {
                    //Stock engineStock = new Stock
                    //{
                    //    ProductId = engineProduct.Id,
                    //    WarehouseId = new Guid("dbe9af41-a908-40f5-8460-27bb1dc1d454"),
                    //    Quantity = 1,
                    //};

                    //_context.Stocks.Add(engineStock);

                    //produce it, call engine service
                    var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("rabbitmq://localhost/produce-engine-queue"));

                    await endpoint.Send(order);
                }
                //go assemble
                else
                {
                    //var stockProduct = _context.Stocks.Where(x => x.ProductId == engineProduct.Id);

                    //if (stockProduct != null)
                    //{
                    //    var currentStockQuantity = stockProduct.FirstOrDefault()?.Quantity;

                    //    stockProduct.ExecuteUpdate(s => s.SetProperty(b => b.Quantity, currentStockQuantity + 1));
                    //}

                    //call assembly service
                    var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("rabbitmq://localhost/assembly-service-queue"));

                    await endpoint.Send(order);
                }
            }

            var chassisProduct = _context.Products.FirstOrDefault(x => x.Id == order.ChassisId);

            if (chassisProduct != null)
            {
                //does stock exist for chassis?
                var hasStock = _context.Stocks.Where(x => x.ProductId == chassisProduct.Id);

                //no, add
                if (!hasStock.Any())
                {
                    Stock chassisStock = new Stock
                    {
                        ProductId = chassisProduct.Id,
                        WarehouseId = new Guid("dbe9af41-a908-40f5-8460-27bb1dc1d454"),
                        Quantity = 1,
                    };

                    _context.Stocks.Add(chassisStock);
                }
                //update
                else
                {
                    var stockProduct = _context.Stocks.Where(x => x.ProductId == chassisProduct.Id);

                    if (stockProduct != null)
                    {
                        var currentStockQuantity = stockProduct.FirstOrDefault()?.Quantity;

                        stockProduct.ExecuteUpdate(s => s.SetProperty(b => b.Quantity, currentStockQuantity + 1));
                    }
                }
            }

            var optionPackProduct = _context.Products.FirstOrDefault(x => x.Id == order.OptionPackId);

            if (optionPackProduct != null)
            {
                //does stock exist for engine?
                var hasStock = _context.Stocks.Where(x => x.ProductId == optionPackProduct.Id);

                //no, add
                if (!hasStock.Any())
                {
                    Stock optionPackStock = new Stock
                    {
                        ProductId = optionPackProduct.Id,
                        WarehouseId = new Guid("dbe9af41-a908-40f5-8460-27bb1dc1d454"),
                        Quantity = 1,
                    };

                    _context.Stocks.Add(optionPackStock);
                }
                //update
                else
                {
                    var stockProduct = _context.Stocks.Where(x => x.ProductId == optionPackProduct.Id);

                    if (stockProduct != null)
                    {
                        var currentStockQuantity = stockProduct.FirstOrDefault()?.Quantity;

                        stockProduct.ExecuteUpdate(s => s.SetProperty(b => b.Quantity, currentStockQuantity + 1));
                    }
                }
            }

            return await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "An issue occured while trying to create order!");
            throw;
        }
    }


    public async Task<int> CreateProduct(Product product)
    {
        try
        {
            if(Enum.IsDefined(typeof(ProductType), product.Type))
            {
                _context.Products.Add(product);
                return await _context.SaveChangesAsync();
            }

            return 0;
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "An issue occured while trying to create order!");
            throw;
        }
    }

    public async Task<int> CreateWarehouse(Warehouse warehouse)
    {
        try
        {
            _context.Warehouses.Add(warehouse);
            return await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "An issue occured while trying to create order!");
            throw;
        }
    }

    public Task<Warehouse> FindAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Warehouse> GetWarehouse(int id)
    {
        throw new NotImplementedException();
    }
}