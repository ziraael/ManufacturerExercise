using Microsoft.AspNet.SignalR;

namespace ChassisService.Api.Hubs;

public class ChassisHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message).ConfigureAwait(false);
    }
}