
using OrderService.Domain.Entities;

public interface IOrderRepository
{
    Task<Order> CreateOrder(Order order);
    Task<Order> GetOrder(int id);
    Task<Order> FindAsync(int id);
}