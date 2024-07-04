using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Repositories;

public class OrderRepository: IOrderRepository
{
    private readonly ApplicationDbContext _context;

    public OrderRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Task<Order> CreateOrder(Order order)
    {
        throw new NotImplementedException();
    }

    public Task<Order> FindAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Order> GetOrder(int id)
    {
        throw new NotImplementedException();
    }
}