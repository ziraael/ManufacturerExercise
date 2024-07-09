using ChassisService.Domain.Entities;
using EngineService.Domain.Entities;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OptionPackService.Domain.Entities;
using OrderService.Domain.Entities;
using System.Linq;
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

    public async Task<int> AddProductToStock(Engine? engine, Chassis? chassis, OptionPack? optionPack)
    {
        try
        {
            ProductDTO product = null;

            if(engine != null)
            {
                product = engine;
            }
            else if (chassis != null)
            {
                product = chassis;
            }
            else if (optionPack != null)
            {
                product = optionPack;
            }

            var hasStock = _context.Stocks.Where(x => x.ProductId == product.ProductId).FirstOrDefault();
            int savedChanges = 0;

            StockDTO stockDto = new StockDTO();

            if (hasStock == null)
            {
                Stock stock = new Stock
                {
                    ProductId = product.ProductId,
                    WarehouseId = new Guid("dbe9af41-a908-40f5-8460-27bb1dc1d454"),
                    Quantity = 1
                };

                _context.Stocks.Add(stock);
                savedChanges = _context.SaveChanges();

                stockDto = new StockDTO
                {
                    ProductId = stock.ProductId,
                    WarehouseId = stock.WarehouseId,
                    Quantity = stock.Quantity,
                    OrderId = product.OrderId
                };
            }
            else
            {
                hasStock.Quantity += 1;
                _context.Stocks.Update(hasStock);
                savedChanges = _context.SaveChanges();

                stockDto = new StockDTO
                {
                    ProductId = hasStock.ProductId,
                    WarehouseId = hasStock.WarehouseId,
                    Quantity = hasStock.Quantity,
                    OrderId = product.OrderId
                };
            }

            await AssembleVehicle(stockDto);
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

                        await AssembleVehicle(stock);
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

                        await AssembleVehicle(stock);
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
                        await AssembleVehicle(stock);
                        //isForAssembly.Add(stock);
                    }
                }

                //assemble parts
                //if (isForAssembly.Count > 0)
                //{
                //    AssembleVehicle(isForAssembly);
                //}
            }

            return true;
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
            var vehicleExists = await (_context.AssembledVehicleStocks.FirstOrDefaultAsync(x => x.OrderId == stock.OrderId));
            var engine = await (_context.Products.FirstOrDefaultAsync(x => x.Id == stock.ProductId && x.Type == ProductType.Engine));
            var chassis = await (_context.Products.FirstOrDefaultAsync(x => x.Id == stock.ProductId && x.Type == ProductType.Chassis));
            var optionPack = await (_context.Products.FirstOrDefaultAsync(x => x.Id == stock.ProductId && x.Type == ProductType.OptionPack));
            AssembledVehicleStock avs = new AssembledVehicleStock();

            //create assemble vehicle
            if (vehicleExists == null)
            {
                avs = new AssembledVehicleStock
                {
                    EngineId = engine?.Id,
                    ChassisId = chassis?.Id,
                    OptionPackId = optionPack?.Id,
                    IsAvailable = true,
                    OrderId = stock.OrderId,
                };

                _context.AssembledVehicleStocks.Add(avs);
            }
            //update assembled vehicle with parts
            else
            {
                if (engine?.Id != null)
                {
                    vehicleExists.EngineId = engine?.Id;
                }

                if (chassis?.Id != null)
                {
                    vehicleExists.ChassisId = chassis?.Id;
                }

                if (optionPack?.Id != null)
                {
                    vehicleExists.OptionPackId = optionPack?.Id;
                }

                _context.AssembledVehicleStocks.Update(vehicleExists);
            }

            //reduce from stock since im using it to assemble a vehicle
            if (engine?.Id != null)
            {
                ReduceFromStock(avs.EngineId ?? vehicleExists?.EngineId);
            }
            else if (chassis?.Id != null)
            {
                ReduceFromStock(avs.ChassisId ?? vehicleExists?.ChassisId);
            }
            else if (optionPack?.Id != null)
            {
                ReduceFromStock(avs.OptionPackId ?? vehicleExists?.OptionPackId);
            }

            return await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "An issue occured while trying to check for assembled vehicle!");
            throw;
        }
    }

    private async void ReduceFromStock(Guid? id)
    {
        try
        {
            if (id != null)
            {
                //reduce from stock since im using it to assemble a vehicle
                var productInStock = await (_context.Stocks.FirstOrDefaultAsync(x => x.ProductId == id));

                if (productInStock != null)
                {
                    if (productInStock.Quantity > 1)
                    {
                        productInStock.Quantity -= 1;
                    }
                    else
                    {
                        _context.Stocks.Remove(productInStock);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "An issue occured while trying to check for assembled vehicle!");
            throw;
        }
    }
}