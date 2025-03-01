using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatClient.MVVM.Models
{
    public class HttpClientSingleton
    {
        
        private static readonly HttpClient _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://192.168.68.131:5206/api/")
        };
         
        public static HttpClient Instance => _httpClient;
        
        public static void SetAuthToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
        }

       

    }
}
