using OrderService.Domain.Entities;
using EngineService.Domain.Entities;

public interface IEngineRepository
{
    Task<Engine> CreateEngine(Order order);
    Task<bool> GetEngineProductionStatus(Guid orderId);
}