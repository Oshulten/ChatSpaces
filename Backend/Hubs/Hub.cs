using Microsoft.AspNetCore.SignalR;

namespace Backend.Hubs;

public class Hub : Hub<IClient>
{
    public override async Task OnConnectedAsync()
    {
        await Clients.All.ReceiveNotification($"Thank you for connecting, {Context.ConnectionId}");
    }
}