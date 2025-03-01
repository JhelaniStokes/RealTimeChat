using Microsoft.AspNetCore.SignalR.Client;
using RealTimeChatClient.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatClient.Services
{
    public class SignalRClient
    {
        private HubConnection connection;
        private readonly AuthService authService;
        public event Action<string, MessageModel> OnMessageReceived;
        public event Action GroupAdded;

       

        public async Task StartConnectionAsync()
        {
            connection = new HubConnectionBuilder().WithUrl("http://192.168.68.131:5206/chathub", options =>
            options.AccessTokenProvider = async () => AuthService.Instance.AuthToken).Build();
            await connection.StartAsync();

            connection.On<string, MessageModel>("ReceiveMessage", (user, message) =>
            {
                OnMessageReceived?.Invoke(user, message);
            });
            connection.On("GroupAdded", ()=>
            GroupAdded?.Invoke());

        }
        

        public async Task SendMessage(int groupId, string message)
        {
            if(connection.State == HubConnectionState.Connected)
            {
                await connection.InvokeAsync("SendMessage", groupId, message);
            }
        }

        public async Task JoinGroup(int groupId)
        {
            if(connection.State == HubConnectionState.Connected)
            {
                await connection.InvokeAsync("JoinGroup", groupId);
            }
        }
        public async Task LeaveGroup(int groupId)
        {
            if(connection.State == HubConnectionState.Connected)
            {
                await connection.InvokeAsync("LeaveGroup", groupId);
            }
        }
    }
}
