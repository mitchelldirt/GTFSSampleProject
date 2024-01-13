using Microsoft.AspNetCore.SignalR;

namespace SingalRServer.Hubs;

public class UserHub : Hub
{
    public static int TotalViews { get; set; } = 0;

    public async Task NewWindowLoaded()
    {
        TotalViews++;
        // Send an update to all client
        await Clients.All.SendAsync("updateTotalViews", TotalViews);
    }
}