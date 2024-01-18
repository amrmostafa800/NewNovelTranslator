using Web.Services;

namespace Web;

public class CurrentUser
{
    private readonly UserService _userService;

    public CurrentUser(UserService userService)
    {
        _userService = userService;
    }

    public static string? Username { get; set; }
    
    public void SubscribeToEvents()
    {
        _userService.OnLogin += SaveCurrentUser;
    }

    private void SaveCurrentUser(string? email)
    {
        Username = email; // In This App Email And Username Is Same By Default
    }
}