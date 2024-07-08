using EngineService.Domain.Entities;
using OrderService.Domain.Entities;
using WarehouseService.Domain.DTOs;
using WarehouseService.Domain.Entities;

public interface IWarehouseRepository
{
    Task<int> CreateWarehouse(Warehouse warehouse);
    Task<int> CreateProduct(Product product);
    Task<int> AssembleVehicle(StockDTO stock);
    Task<bool> CheckStock(Order order);
    Task<int> AddEngineToStock(Engine order);
    bool CheckAssembledVehicleStock(Order order);
}