using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RealTimeChat.Models;
using System.Diagnostics;

namespace RealTimeChat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly AppDbContext dbContext;
        public ChatHub(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task SendMessage(int groupId, string message)
        {
            
            var senderUsername = Context.User?.FindFirst("uid").Value;

            if (string.IsNullOrEmpty(senderUsername)) return;

            
            var sender = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == int.Parse(senderUsername));
            if (sender == null) return;
            var chatMessage = new Message
            {
                SenderId = sender.Id,
                GroupId = groupId,
                Content = message,
                SentAt = DateTime.UtcNow,
                SenderName = sender.Username
            };

            dbContext.Messages.Add(chatMessage);
            await dbContext.SaveChangesAsync();

            
            await Clients.Group(groupId.ToString()).SendAsync("ReceiveMessage", sender.Username, chatMessage);
        }

        public async Task JoinGroup(int groupId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupId.ToString());
        }
        public async Task LeaveGroup(int groupId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId.ToString());
        }
        public override Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier; // SignalR's mapping
            Console.WriteLine($"User connected: ConnectionId={Context.ConnectionId}, UserIdentifier={userId}");
            return base.OnConnectedAsync();
        }

    }
}
