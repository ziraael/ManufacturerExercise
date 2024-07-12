using OrderService.Domain.Entities;
using OptionPackService.Domain.Entities;

public interface IOptionPackRepository
{
    Task<OptionPack> CreateOptionPack(Order order);
    Task<bool> GetOptionPackProductionStatus(Guid orderId);
}