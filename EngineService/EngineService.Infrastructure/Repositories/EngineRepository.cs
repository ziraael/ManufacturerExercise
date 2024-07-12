using Microsoft.Extensions.Logging;
using OrderService.Domain.Entities;
using EngineService.Domain.Entities;

namespace EngineService.Infrastructure.Repositories;

public class EngineRepository : IEngineRepository
{
    private readonly ApplicationDbContext _context;
    private ILogger _logger;

    public EngineRepository(ApplicationDbContext context, ILogger logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger;
    }
    public async Task<Engine> CreateEngine(Order order)
    {
        try
        {
            Engine engine = new Engine
            {
                ProductId = order.EngineId,
                StartedProduction = DateTime.Now,
                EndedProduction = null,
                OrderId = order.Id,
            };

            _context.Engines.Add(engine);
            await _context.SaveChangesAsync();

            //sleep 15sec, simulate engine producing
            Thread.Sleep(15000);

            engine.EndedProduction = DateTime.Now;
            await _context.SaveChangesAsync();

            return engine;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An issue occured while trying to create engine!");
            throw;
        }
    }

    public Task<bool> GetEngineProductionStatus(Guid orderId)
    {
        try
        {
            var hasProductionEnded = _context.Engines.SingleOrDefault(x => x.OrderId == orderId)?.EndedProduction;

            if(hasProductionEnded == null)
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An issue occured while trying to get engine production status!");
            throw;
        }
    }
}