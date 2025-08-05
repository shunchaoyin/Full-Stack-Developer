using Microsoft.AspNetCore.SignalR.Client;
using Shared.Models;

namespace FrontEndApp.Services;

public class ChatService : IAsyncDisposable
{
    private HubConnection? _hubConnection;
    private readonly string _hubUrl = "http://localhost:5275/chathub";

    public event Action<ChatMessage>? OnMessageReceived;

    public async Task StartAsync()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(_hubUrl)
            .Build();

        _hubConnection.On<ChatMessage>("ReceiveMessage", (message) =>
        {
            OnMessageReceived?.Invoke(message);
        });

        await _hubConnection.StartAsync();
    }

    public async Task SendMessage(ChatMessage chatMessage)
    {
        if (_hubConnection is not null && _hubConnection.State == HubConnectionState.Connected)
        {
            await _hubConnection.SendAsync("SendMessage", chatMessage);
        }
    }

    public async Task StopAsync()
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.StopAsync();
        }
    }

    public bool IsConnected =>
        _hubConnection?.State == HubConnectionState.Connected;

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.DisposeAsync();
        }
    }
}
