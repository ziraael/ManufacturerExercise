using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderService.Domain.Entities;
using System.Reflection;

namespace OrderService.Infrastructure.Repositories;

public class OrderRepository: IOrderRepository
{
    private readonly ApplicationDbContext _context;
    private ILogger _logger;
    public OrderRepository(ApplicationDbContext context, ILogger logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger;
    }

    public async Task<Order?> GetOrderById(Guid orderId)
    {
        try
        {
            return await (_context.Orders.FirstOrDefaultAsync(x => x.Id == orderId));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An issue occured while trying to get order!");
            throw;
        }
    }

    public async Task<List<Order>> GetAllOrders()
    {
        try
        {
            return await _context.Orders.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An issue occured while trying to get orders!");
            throw;
        }
    }
    public bool ChangeOrderStatus(Guid orderId, string type, bool statusValue)
    {
        try
        {
            var order = _context.Orders.SingleOrDefault(x => x.Id == orderId);

            if (order != null)
            {
                if (order.IsCanceled == false)
                {
                    PropertyInfo propertyInfo = typeof(Order).GetProperty(type, BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo != null && propertyInfo.PropertyType == typeof(bool))
                    {
                        if (type == "IsCanceled")
                        {
                            if (!order.IsReadyForCollection == false)
                            {
                                return false;
                            }
                        }

                        propertyInfo.SetValue(order, statusValue);

                        _context.SaveChanges();
                        return true;
                    }
                }
                _logger.LogError("Couldn't make order ready for collection since it was canceled!");
                return false;
            }
            else
            {
                _logger.LogError("Order not found!");
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An issue occured while trying to change order status!");
            throw;
        }
    }

    public bool CreateOrder(Order order)
    {
        try
        {
            order.OrderDate = DateTime.Now;
            order.IsReadyForCollection = false;
            order.IsCanceled = false;

            _context.Orders.Add(order);
            _context.SaveChanges();
            return true;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "An issue occured while trying to create order!");
            throw;
        }
    }
}