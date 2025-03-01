using RealTimeChatClient.MVVM.Models;
using RealTimeChatClient.MVVM.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatClient.Services
{
    public class AuthService
    {
        private static AuthService _instance;
        private static readonly object _lock = new object();

        private readonly HttpClient _httpClient;
        private string _authToken;

        // ✅ Singleton Accessor
        public static AuthService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock) // Ensure thread safety
                    {
                        if (_instance == null)
                        {
                            _instance = new AuthService();
                        }
                    }
                }
                return _instance;
            }
        }

        // ✅ Private Constructor (Prevents instantiation from outside)
        private AuthService()
        {
            _httpClient = HttpClientSingleton.Instance;
        }

        // ✅ Global Token Getter (Other classes can access the latest token)
        public string AuthToken => _authToken;

        public async Task<bool> AuthenticateUser(string username, string password)
        {
            var loginRequest = new LoginRequest { Password = password, Username = username };
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("auth/login", loginRequest);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                _authToken = result.Token; // ✅ Store token globally

                // ✅ Apply Token to HttpClient for Future Requests
                HttpClientSingleton.SetAuthToken(_authToken);
            }
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RegisterUser(string username, string password)
        {
            var registerRequest = new RegisterRequest { PasswordHash = password, Username = username };
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("auth/register", registerRequest);
            return response.IsSuccessStatusCode;
        }

        // ✅ Logout Method (Clears Token)
        public void Logout()
        {
            _authToken = null;
            HttpClientSingleton.SetAuthToken(null);
        }
    }
}
