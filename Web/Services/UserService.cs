using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using Web.Dto;

namespace Web.Services
{
    public class UserService
    {
        private readonly HttpClient _client;

        public UserService(HttpClient client)
        {
            _client = client;
        }

        public async Task<bool> Register(string email,string password)
        {
            AccountDto register = new()
            {
                email= email,
                password= password
            };

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5000/register");

            request.Headers.Add("accept", "*/*");

            request.Content = new StringContent(JObject.FromObject(register).ToString());
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await _client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> Login(string email, string password)
        {
            AccountDto login = new()
            {
                email = email,
                password = password
            };

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5000/login");

            request.Headers.Add("accept", "*/*");

            request.Content = new StringContent(JObject.FromObject(login).ToString());
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await _client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
