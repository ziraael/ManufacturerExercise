using Microsoft.AspNetCore.SignalR;

namespace OrderService.Api.Hubs;

public class OrderHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message).ConfigureAwait(false);
    }
}