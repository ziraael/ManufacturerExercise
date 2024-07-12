using OrderService.Domain.Entities;
using ChassisService.Domain.Entities;

public interface IChassisRepository
{
    Task<Chassis> CreateChassis(Order order);
    Task<bool> GetChassisProductionStatus(Guid orderId);
}