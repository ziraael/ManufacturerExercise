using OrderService.Domain.Entities;
using ChassisService.Domain.Entities;
using ChassisService.Infrastructure;
using Microsoft.Extensions.Logging;

namespace WarehouseService.Infrastructure.Repositories;

public class ChassisRepository : IChassisRepository
{
    private readonly ApplicationDbContext _context;
    private ILogger _logger;


    public ChassisRepository(ApplicationDbContext context, ILogger logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger;
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

            //inform consumer to signal the front that its ready
            //await _hubContext.Clients.All.SendAsync("ReceiveMessage", "the weatherman", "").ConfigureAwait(false);

            return chassis;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An issue occured while trying to create chassis!");
            throw;
        }
    }

    public Task<bool> GetChassisProductionStatus(Guid orderId)
    {
        try
        {
            var hasProductionEnded = _context.Chasses.SingleOrDefault(x => x.OrderId == orderId)?.EndedProduction;

            if (hasProductionEnded == null)
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An issue occured while trying to get chassis production status!");
            throw;
        }
    }
}