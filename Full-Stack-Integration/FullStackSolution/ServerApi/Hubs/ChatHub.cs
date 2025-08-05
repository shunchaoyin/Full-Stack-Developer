using Microsoft.AspNetCore.SignalR;
using Shared.Models;

namespace ServerApi.Hubs;

public class ChatHub : Hub
{
    public async Task SendMessage(ChatMessage chatMessage)
    {
        chatMessage.Timestamp = DateTime.UtcNow;
        await Clients.All.SendAsync("ReceiveMessage", chatMessage);
    }

    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "ChatRoom");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "ChatRoom");
        await base.OnDisconnectedAsync(exception);
    }
}
