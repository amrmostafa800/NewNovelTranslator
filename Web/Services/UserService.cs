using System.Net.Http.Json;
using Web.Dto;

namespace Web.Services;

public class UserService
{
    private readonly HttpClient _client;

    public UserService(HttpClient client)
    {
        _client = client;
    }

    public event EventHandler? OnLogin;
    public event EventHandler? OnLogout;

    public async Task<bool> Register(string email, string password)
    {
        AccountDto register = new()
        {
            email = email,
            password = password
        };

        var response = await _client.PostAsJsonAsync("/register", register);
        if (response.IsSuccessStatusCode) return true;
        return false;
    }

    public async Task<bool> Login(string email, string password)
    {
        AccountDto login = new()
        {
            email = email,
            password = password
        };

        var response = await _client.PostAsJsonAsync("/login?useCookies=true", login);
        var coockies = response.Headers.Where(h => h.Key == "Cookie");
        if (response.IsSuccessStatusCode)
        {
            OnLogin?.Invoke(this, EventArgs.Empty);
            return true;
        }

        return false;
    }

    public async Task<bool> IsAuth()
    {
        var response = await _client.GetAsync("api/Validation/IsAuth");
        if (response.IsSuccessStatusCode)
        {
            if (await response.Content.ReadAsStringAsync() == "true") return true;
            return false;
        }

        throw new Exception("Server Error");
    }

    public async Task<bool> LogOut()
    {
        var response = await _client.GetAsync("api/Validation/Logout");
        if (response.IsSuccessStatusCode)
        {
            OnLogout?.Invoke(this, EventArgs.Empty);
            return true;
        }

        return false;
    }
}