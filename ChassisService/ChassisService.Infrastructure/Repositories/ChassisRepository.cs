using MassTransit;
using OrderService.Domain.Entities;
using ChassisService.Domain.Entities;
using ChassisService.Infrastructure;

namespace WarehouseService.Infrastructure.Repositories;

public class ChassisRepository : IChassisRepository
{
    private readonly ApplicationDbContext _context;
    //private ILogger _logger;
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public ChassisRepository(ApplicationDbContext context/*, ILogger logger*/, ISendEndpointProvider sendEndpointProvider)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        //_logger = logger;
        _sendEndpointProvider = sendEndpointProvider ?? throw new ArgumentNullException();
    }
    public async Task<Chassis> CreateChassis(Order order)
    {
        try
        {
            Chassis chassis = new Chassis
            {
                ProductId = order.ChassisId,
                StartedProduction = DateTime.Now,
                EndedProduction = null,
                OrderId = order.Id,
            };

            _context.Chasses.Add(chassis);
            await _context.SaveChangesAsync();

            //sleep 15sec, simulate chassis producing
            Thread.Sleep(15000);

            chassis.EndedProduction = DateTime.Now;
            await _context.SaveChangesAsync();

            return chassis;
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "An issue occured while trying to create order!");
            throw;
        }
    }
}