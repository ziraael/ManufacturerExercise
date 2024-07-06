using OrderService.Domain.Entities;
using System.Reflection;

namespace OrderService.Infrastructure.Repositories;

public class OrderRepository: IOrderRepository
{
    private readonly ApplicationDbContext _context;

    public OrderRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public bool CancelOrder(Guid orderId)
    {
        try
        {
            var order = _context.Orders.SingleOrDefault(x => x.Id == orderId);

            if (order != null && order.IsReadyForCollection == false)
            {
                order.IsCanceled = true;
                _context.Orders.Update(order);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
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
                PropertyInfo propertyInfo = typeof(Order).GetProperty(type, BindingFlags.Public | BindingFlags.Instance);

                // Check if the property exists and is of type bool
                if (propertyInfo != null && propertyInfo.PropertyType == typeof(bool))
                {
                    // Update the property value
                    propertyInfo.SetValue(order, statusValue);

                    // Save the changes using Entity Framework
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
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
            throw;
        }
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