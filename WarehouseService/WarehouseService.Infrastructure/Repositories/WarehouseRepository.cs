using EngineService.Domain.Entities;
using MassTransit;
using MediatR;
using OrderService.Domain.Entities;
using System.Text.Json;
using WarehouseService.Domain.DTOs;
using WarehouseService.Domain.Entities;

namespace WarehouseService.Infrastructure.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly ApplicationDbContext _context;
    //private ILogger _logger;
    private readonly ISendEndpointProvider _sendEndpointProvider;
    private IMediator _mediator;
    public WarehouseRepository(ApplicationDbContext context/*, ILogger logger*/, ISendEndpointProvider sendEndpointProvider, IMediator mediator)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        //_logger = logger;
        _sendEndpointProvider = sendEndpointProvider ?? throw new ArgumentNullException();
        _mediator = mediator;
    }

    public async Task<int> AddEngineToStock(Engine engine)
    {
        try
        {
            var hasStock = _context.Stocks.Where(x => x.ProductId == engine.Id).FirstOrDefault();
            var partForAssemble = new List<StockDTO>();

            if (hasStock == null)
            {
                Stock stock = new Stock
                {
                    ProductId = engine.ProductId,
                    WarehouseId = new Guid("dbe9af41-a908-40f5-8460-27bb1dc1d454"),
                    Quantity = 1
                };

                _context.Stocks.Add(stock);

                StockDTO stockDto = new StockDTO
                {
                    ProductId = stock.ProductId,
                    WarehouseId = stock.WarehouseId,
                    Quantity = stock.Quantity,
                    OrderId = engine.OrderId
                };

                partForAssemble.Add(stockDto);
            }
            else
            {
                hasStock.Quantity += 1;
                _context.Stocks.Update(hasStock);

                StockDTO stockDto = new StockDTO
                {
                    ProductId = hasStock.ProductId,
                    WarehouseId = hasStock.WarehouseId,
                    Quantity = hasStock.Quantity,
                    OrderId = engine.OrderId
                };

                partForAssemble.Add(stockDto);
            }

            var savedChanges = await _context.SaveChangesAsync();

            AssembleVehicle(partForAssemble);

            return savedChanges;
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "An issue occured while trying to create product!");
            throw;
        }
    }

    public async Task<bool> CheckStock(Order order)
    {
        try
        {
            //proceed if there arent any assembled vehicles
            if (!CheckAssembledVehicleStock(order)) {
                var engineProduct = _context.Products.FirstOrDefault(x => x.Id == order.EngineId);

                List<StockDTO> isForAssembly = new List<StockDTO>();

                if (engineProduct != null)
                {
                    //does stock exist for engine?
                    var hasStock = _context.Stocks.Where(x => x.ProductId == engineProduct.Id).FirstOrDefault();

                    //no, produce it
                    if (hasStock == null)
                    {
                        var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("rabbitmq://localhost/produce-engine-queue"));

                        await endpoint.Send(order);
                    }
                    //send it for assembly
                    else
                    {
                        StockDTO stock = new StockDTO
                        {
                            ProductId = hasStock.ProductId,
                            WarehouseId = hasStock.WarehouseId,
                            Quantity = hasStock.Quantity,
                            OrderId = order.Id
                        };

                        isForAssembly.Add(stock);
                    }
                }

                var chassisProduct = _context.Products.FirstOrDefault(x => x.Id == order.ChassisId);

                if (chassisProduct != null)
                {
                    //does stock exist for chassis?
                    var hasStock = _context.Stocks.Where(x => x.ProductId == chassisProduct.Id).FirstOrDefault();

                    //no, produce it
                    if (hasStock == null)
                    {
                        var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("rabbitmq://localhost/produce-chassis-queue"));

                        await endpoint.Send(order);
                    }
                    //go assemble, call assembly service
                    else
                    {
                        StockDTO stock = new StockDTO
                        {
                            ProductId = hasStock.ProductId,
                            WarehouseId = hasStock.WarehouseId,
                            Quantity = hasStock.Quantity,
                            OrderId = order.Id
                        };

                        isForAssembly.Add(stock);
                    }
                }

                var optionPackProduct = _context.Products.FirstOrDefault(x => x.Id == order.OptionPackId);

                if (optionPackProduct != null)
                {
                    //does stock exist for engine?
                    var hasStock = _context.Stocks.Where(x => x.ProductId == optionPackProduct.Id).FirstOrDefault();

                    //no, produce it
                    if (hasStock == null)
                    {
                        var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("rabbitmq://localhost/produce-optionpack-queue"));

                        await endpoint.Send(order);
                    }
                    //go assemble, call assembly service
                    else
                    {
                        StockDTO stock = new StockDTO
                        {
                            ProductId = hasStock.ProductId,
                            WarehouseId = hasStock.WarehouseId,
                            Quantity = hasStock.Quantity,
                            OrderId = order.Id
                        };

                        isForAssembly.Add(stock);
                    }
                }

                //assemble parts
                if (isForAssembly.Count > 0)
                {
                    AssembleVehicle(isForAssembly);
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "An issue occured while trying to create order!");
            throw;
        }
    }

    public async void AssembleVehicle(List<StockDTO> obj)
    {
        var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("rabbitmq://localhost/assemble-vehicle-queue"));

        for (var i = 0; i < obj.Count; i++)
        {
            await endpoint.Send(obj[i]);
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
            //_logger.LogError(ex, "An issue occured while trying to create product!");
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
            //_logger.LogError(ex, "An issue occured while trying to create warehouse!");
            throw;
        }
    }

    public bool CheckAssembledVehicleStock(Order order)
    {
        try
        {
            var checkForStock = _context.AssembledVehicleStocks.FirstOrDefault(x => x.EngineId == order.EngineId && x.ChassisId == order.ChassisId && x.OptionPackId == order.OptionPackId);

            if (checkForStock != null)
            {
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "An issue occured while trying to check for assembled vehicle!");
            throw;
        }
    }

    public async Task<int> AssembleVehicle(StockDTO stock)
    {
        try
        {
            var vehicleExists = _context.AssembledVehicleStocks.FirstOrDefault(x => x.OrderId == stock.OrderId);
            var engineId = _context.Products.FirstOrDefault(x => x.Id == stock.ProductId && x.Type == ProductType.Engine)?.Id;
            var chassisId = _context.Products.FirstOrDefault(x => x.Id == stock.ProductId && x.Type == ProductType.Chassis)?.Id;
            var optionPackId = _context.Products.FirstOrDefault(x => x.Id == stock.ProductId && x.Type == ProductType.OptionPack)?.Id;

            //create assemble vehicle
            if (vehicleExists == null) {
                AssembledVehicleStock avs = new AssembledVehicleStock
                {
                    EngineId = engineId,
                    ChassisId = chassisId,
                    OptionPackId = optionPackId,
                    IsAvailable = true,
                    OrderId = stock.OrderId,
                };

                _context.AssembledVehicleStocks.Add(avs);
            }
            //update assembled vehicle with parts
            else
            {
                if (engineId != null)
                {
                    vehicleExists.EngineId = engineId;
                }

                if (chassisId != null)
                {
                    vehicleExists.ChassisId = chassisId;
                }

                if (optionPackId != null)
                {
                    vehicleExists.OptionPackId = optionPackId;
                }

                _context.AssembledVehicleStocks.Update(vehicleExists);
            }

            return await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "An issue occured while trying to check for assembled vehicle!");
            throw;
        }
    }
}