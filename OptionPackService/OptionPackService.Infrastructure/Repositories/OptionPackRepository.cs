using Microsoft.Extensions.Logging;
using OptionPackService.Domain.Entities;
using OrderService.Domain.Entities;

namespace OptionPackService.Infrastructure.Repositories;

public class OptionPackRepository : IOptionPackRepository
{
    private readonly ApplicationDbContext _context;
    private ILogger _logger;

    public OptionPackRepository(ApplicationDbContext context, ILogger logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger;
    }
    public async Task<OptionPack> CreateOptionPack(Order order)
    {
        try
        {
            OptionPack engine = new OptionPack
            {
                ProductId = order.OptionPackId,
                StartedProduction = DateTime.Now,
                EndedProduction = null,
                OrderId = order.Id,
            };

            _context.OptionPacks.Add(engine);
            await _context.SaveChangesAsync();

            //sleep 15sec, simulate engine producing
            Thread.Sleep(15000);

            engine.EndedProduction = DateTime.Now;
            await _context.SaveChangesAsync();

            return engine;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An issue occured while trying to create option pack!");
            throw;
        }
    }

    public Task<bool> GetOptionPackProductionStatus(Guid orderId)
    {
        try
        {
            var hasProductionEnded = _context.OptionPacks.SingleOrDefault(x => x.OrderId == orderId)?.EndedProduction;

            if (hasProductionEnded == null)
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An issue occured while trying to get option pack production status!");
            throw;
        }
    }
}