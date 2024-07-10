
using OrderService.Domain.Entities;

public interface IOrderRepository
{
    bool CreateOrder(Order order);
    bool ChangeOrderStatus(Guid orderId, string type, bool statusValue);
    Task<Order?> GetOrderById(Guid orderId);
    Task<List<Order>> GetAllOrders();

}