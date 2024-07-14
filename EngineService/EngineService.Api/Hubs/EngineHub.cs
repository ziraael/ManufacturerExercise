using Microsoft.AspNetCore.SignalR;

namespace EngineService.Api.Hubs;

public class EngineHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        //await Clients.All.SendAsync("EngineReady", user, message).ConfigureAwait(false);
    }
}