using System.Net.Http.Json;
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

        public async Task<bool> Register(string email, string password)
        {
            AccountDto register = new()
            {
                email = email,
                password = password
            };

            HttpResponseMessage response = await _client.PostAsJsonAsync("/register", register);
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

            HttpResponseMessage response = await _client.PostAsJsonAsync("/login?useCookies=true", login);
            var coockies = response.Headers.Where(h => h.Key == "Cookie");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
