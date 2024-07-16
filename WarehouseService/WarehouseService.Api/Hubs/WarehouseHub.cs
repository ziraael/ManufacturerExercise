using Microsoft.AspNetCore.SignalR;

namespace WarehouseService.Api.Hubs;

public class WarehouseHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message).ConfigureAwait(false);
    }
}