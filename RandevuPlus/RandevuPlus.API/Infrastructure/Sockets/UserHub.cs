using Microsoft.AspNetCore.SignalR;
using RandevuPlus.API.Shared.Models;

namespace RandevuPlus.API.Infrastructure.Sockets
{
    public class UserHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            if(userId != null)
            {
                OnlineUsers.Users.TryAdd(userId, Context.ConnectionId);
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier;
            if(userId != null)
            {
                OnlineUsers.Users.TryRemove(userId, out _);
            }
            return base.OnDisconnectedAsync(exception);
        }

        public Task SendMessageToUser(string userId)
        {
            if (OnlineUsers.Users.TryGetValue(userId, out var connectionId))
            {
                return Clients.Client(connectionId).SendAsync("MessageReceived");
            }

            return Task.CompletedTask; 
        }
    }
}
