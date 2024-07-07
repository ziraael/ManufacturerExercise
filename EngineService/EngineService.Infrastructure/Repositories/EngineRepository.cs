using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderService.Domain.Entities;
using EngineService.Domain.Entities;

namespace WarehouseService.Infrastructure.Repositories;

public class EngineRepository : IEngineRepository
{
    private readonly ApplicationDbContext _context;
    //private ILogger _logger;
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public EngineRepository(ApplicationDbContext context/*, ILogger logger*/, ISendEndpointProvider sendEndpointProvider)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        //_logger = logger;
        _sendEndpointProvider = sendEndpointProvider ?? throw new ArgumentNullException();
    }
    public async Task<Engine> CreateEngine(Order order)
    {
        try
        {
            Engine engine = new Engine
            {
                StartedProduction = DateTime.Now,
                EndedProduction = DateTime.Now,
                OrderId = order.Id,
            };

            _context.Engines.Add(engine);
            await _context.SaveChangesAsync();
            return engine;
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "An issue occured while trying to create order!");
            throw;
        }
    }
}