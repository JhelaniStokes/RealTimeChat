using RealTimeChatClient.MVVM.Models;
using RealTimeChatClient.MVVM.Models.Dto;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatClient.Services
{
    class GroupServices
    {
        private readonly HttpClient httpClient;  

        public GroupServices()
        {
            httpClient = HttpClientSingleton.Instance;
            //HttpClientSingleton.SetAuthToken("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJsb2dpbnRlc3QiLCJqdGkiOiI1YmNhMGE5ZC03ZWQzLTQwYTMtODA0My0xMjgwNDM2OWZmYmYiLCJ1aWQiOiIxIiwiZXhwIjoxNzQ1ODk4MTYzLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MDc2IiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzA3NiJ9.tbxM0Sj4gTKm0Ob4txLdtG017XR5ljHdIcyOo31a1a0");//TAKE THIS LINE OUT WHEN AUTHSERVICES IS DOEN
        }
        public async Task<List<GroupModel>> GetGroups()
        {
            try
            {

                HttpResponseMessage response = await httpClient.GetAsync("group/fetch");
               

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<GroupModel>>() ?? new List<GroupModel>();
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"🚨 API Error: {response.StatusCode} - {errorContent}");
                    return new List<GroupModel>();
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"❌ Network error: {ex.Message}");
                return new List<GroupModel>();
            }
        }

        public async Task<List<MessageModel>> GetMessages(int groupId)
        {
            try
            {

                HttpResponseMessage response = await httpClient.GetAsync($"chat/fetchmessages/{groupId}");


                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<MessageModel>>() ?? new List<MessageModel>();
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"🚨 API Error: {response.StatusCode} - {errorContent}");
                    return new List<MessageModel>();
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"❌ Network error: {ex.Message}");
                return new List<MessageModel>();
            }
        }

        public async Task CreateGroup(string groupName)
        {
            CreateGroupDto dto = new CreateGroupDto() { GroupName = groupName, IsDm = false };
            await httpClient.PostAsJsonAsync<CreateGroupDto>("group/create", dto);
        }
        public async Task AddToGroup(int groupId, string username)
        {
            AddUserDto dto = new AddUserDto() { Username = username, GroupId = groupId };
            await httpClient.PostAsJsonAsync($"group/adduser", dto);
        }

    }
}
