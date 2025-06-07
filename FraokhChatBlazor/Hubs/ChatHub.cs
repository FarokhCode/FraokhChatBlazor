using FraokhChatBlazor.Client.Model;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace FraokhChatBlazor.Hubs
{
    public class ChatHub : Hub
    {
        private static readonly ConcurrentDictionary<string, string> Users = new();

        public async Task AddUser(string username)
        {
            Users[Context.ConnectionId] = username;
            await Clients.All.SendAsync("UserConnected", username);
            await SendOnlineUsers();
        }

        public async Task RemoveUser(string username)
        {
            Users.TryRemove(Context.ConnectionId, out _);
            await Clients.All.SendAsync("UserDisconnected", username);
            await SendOnlineUsers();
        }

        public async Task SendMessage(string receiverName, string message)
        {
            var sender = Users[Context.ConnectionId];
            var receiver = Users.FirstOrDefault(u => u.Value == receiverName);

            if (receiver.Value != null)
            {
                await Clients.Client(receiver.Key).SendAsync("ReceiveMessage", sender, message);
            }
        }

        private async Task SendOnlineUsers()
        {
            await Clients.All.SendAsync("ReceiveOnlineUsers", Users.Values.ToList());
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (Users.TryRemove(Context.ConnectionId, out var username))
            {
                await Clients.All.SendAsync("UserDisconnected", username);
                await SendOnlineUsers();
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
