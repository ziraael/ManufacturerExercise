using MassTransit;
using Microsoft.Extensions.Logging;
using OptionPackService.Domain.Entities;
using OrderService.Domain.Entities;

namespace OptionPackService.Infrastructure.Repositories;

public class OptionPackRepository : IOptionPackRepository
{
    private readonly ApplicationDbContext _context;
    private ILogger _logger;
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public OptionPackRepository(ApplicationDbContext context, ILogger logger, ISendEndpointProvider sendEndpointProvider)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger;
        _sendEndpointProvider = sendEndpointProvider;
    }
    public async Task<OptionPack> CreateOptionPack(Order order)
    {
        try
        {
            OptionPack option = new OptionPack
            {
                ProductId = order.OptionPackId,
                StartedProduction = DateTime.Now,
                EndedProduction = null,
                OrderId = order.Id,
            };

            _context.OptionPacks.Add(option);
            await _context.SaveChangesAsync();

            //sleep 15sec, simulate engine producing
            Thread.Sleep(15000);

            option.EndedProduction = DateTime.Now;
            await _context.SaveChangesAsync();

            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("rabbitmq://localhost/inform-option-queue"));

            await endpoint.Send(option);

            return option;
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