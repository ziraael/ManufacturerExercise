using ChassisService.Domain.Entities;
using EngineService.Domain.Entities;
using OptionPackService.Domain.Entities;
using OrderService.Domain.Entities;
using WarehouseService.Domain.DTOs;
using WarehouseService.Domain.Entities;

public interface IWarehouseRepository
{
    Task<int> CreateWarehouse(Warehouse warehouse);
    Task<int> CreateProduct(Product product);
    int AssembleVehicle(StockDTO stock);
    Task<bool> CheckStock(Order order);
    Task<int> AddProductToStock(Engine? engine, Chassis? chassis, OptionPack? optionPack);
    bool CheckAssembledVehicleStock(Order order);
    Task<List<Product>> GetProducts();
}