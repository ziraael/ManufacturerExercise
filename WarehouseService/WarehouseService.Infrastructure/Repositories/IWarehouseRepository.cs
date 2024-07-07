﻿using OrderService.Domain.Entities;
using WarehouseService.Domain.Entities;

public interface IWarehouseRepository
{
    Task<int> CreateWarehouse(Warehouse warehouse);
    Task<int> CreateProduct(Product product);
    Task<int> UpdateStock(Order order);
    bool CheckStock(Order order);
    Task<Warehouse> GetWarehouse(int id);
    Task<Warehouse> FindAsync(int id);
}