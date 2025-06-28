using Microsoft.AspNetCore.SignalR;

namespace QuizLlamaServer;

public class TestHub : Hub
{
    public async Task SendMessage(string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", $"Server received: {message}");
    }

    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("ReceiveMessage", "Someone connected!");
        await base.OnConnectedAsync();
    }

}