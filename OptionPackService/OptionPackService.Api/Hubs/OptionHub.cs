using Microsoft.AspNetCore.SignalR;

namespace OptionPackService.Api.Hubs;

public class OptionHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
    }
}