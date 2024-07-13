using ChassisService.Domain.Entities;
using EngineService.Domain.Entities;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OptionPackService.Domain.Entities;
using OrderService.Domain.Entities;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using WarehouseService.Domain.DTOs;
using WarehouseService.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace WarehouseService.Infrastructure.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly ApplicationDbContext _context;
    private ILogger _logger;
    private readonly ISendEndpointProvider _sendEndpointProvider;
    static object _lock = new Object();
    public WarehouseRepository(ApplicationDbContext context, ILogger logger, ISendEndpointProvider sendEndpointProvider, IMediator mediator)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger;
        _sendEndpointProvider = sendEndpointProvider ?? throw new ArgumentNullException();
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

            var hasStock = await _context.Stocks.FirstOrDefaultAsync(x => x.ProductId == product.ProductId);
            int savedChanges = 0;

            StockDTO stockDto = new StockDTO();

            if (hasStock == null)
            {
                //take the first warehouse...
                Stock stock = new Stock
                {
                    ProductId = product.ProductId,
                    WarehouseId = _context.Warehouses.FirstOrDefault().Id,
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

            AssembleVehicle(stockDto);
            return savedChanges;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An issue occured while trying to add product to stock!");
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

                         AssembleVehicle(stock);
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

                         AssembleVehicle(stock);
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
                         AssembleVehicle(stock);
                    }
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An issue occured while trying to check for stock!");
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
            _logger.LogError(ex, "An issue occured while trying to create product!");
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
            _logger.LogError(ex, "An issue occured while trying to create warehouse!");
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
            _logger.LogError(ex, "An issue occured while trying to check for assembled vehicle!");
            throw;
        }
    }

    public int AssembleVehicle(StockDTO stock)
    {
        try
        {
            lock (_lock)
            {
                var vehicleExists = _context.AssembledVehicleStocks.FirstOrDefault(x => x.OrderId == stock.OrderId);
                var engine = _context.Products.FirstOrDefault(x => x.Id == stock.ProductId && x.Type == ProductType.Engine);
                var chassis = _context.Products.FirstOrDefault(x => x.Id == stock.ProductId && x.Type == ProductType.Chassis);
                var optionPack = _context.Products.FirstOrDefault(x => x.Id == stock.ProductId && x.Type == ProductType.OptionPack);
                AssembledVehicleStock avs = new AssembledVehicleStock();

                //reduce from stock since im using it to assemble a vehicle
                if (engine?.Id != null)
                {
                    ReduceFromStock(avs.EngineId ?? engine?.Id);
                }
                else if (chassis?.Id != null)
                {
                    ReduceFromStock(avs.ChassisId ?? chassis?.Id);
                }
                else if (optionPack?.Id != null)
                {
                    ReduceFromStock(avs.OptionPackId ?? optionPack?.Id);
                }

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
                    _context.SaveChanges();
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

                    //sleep 15sec, simulate vehicle assembling
                    Thread.Sleep(15000);
                    _context.AssembledVehicleStocks.Update(vehicleExists);
                    _context.SaveChanges();

                    //make it ready for collection, inform order service
                    if (vehicleExists.EngineId != null && vehicleExists.ChassisId != null && vehicleExists.OptionPackId != null)
                    {
                        MakeReadyForCollection(vehicleExists);
                    }
                }
            }
            return 1;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An issue occured while trying to assemble vehicle!");
            throw;
        }
    }

    private async Task MakeReadyForCollection(AssembledVehicleStock vehicle)
    {
        try
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("rabbitmq://localhost/ready-collection-queue"));

            await endpoint.Send(vehicle);
        }
        catch(Exception ex) { 
        }
    }

    private void ReduceFromStock(Guid? id)
    {
        try
        {
            if (id != null)
            {
                //reduce from stock since im using it to assemble a vehicle
                var productInStock =  _context.Stocks.FirstOrDefault(x => x.ProductId == id);

                if (productInStock != null)
                {
                    if (productInStock.Quantity > 1)
                    {
                        productInStock.Quantity -= 1;
                        _context.Stocks.ExecuteUpdate(s => s.SetProperty(u => u.Quantity,productInStock.Quantity));
                    }
                    else
                    {
                        _context.Stocks.Remove(productInStock);
                        _context.SaveChanges();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An issue occured while trying to reduce stock!");
            throw;
        }
    }
}