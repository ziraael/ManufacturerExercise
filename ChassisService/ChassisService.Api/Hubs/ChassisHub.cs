using Microsoft.AspNetCore.SignalR;

namespace ChassisService.Api.Hubs;

public class ChassisHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
    }
}