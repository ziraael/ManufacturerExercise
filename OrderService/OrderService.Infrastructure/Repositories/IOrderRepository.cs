
using OrderService.Domain.Entities;

public interface IOrderRepository
{
    bool CreateOrder(Order order);
    bool CancelOrder(Guid orderId);
    bool ChangeOrderStatus(Guid orderId, string type, bool statusValue);

    Task<Order> GetOrder(int id);
    Task<Order> FindAsync(int id);
}