using System.Net.Http.Json;
using Web.Enums;
using Web.Models;

namespace Web.Services;

public class NovelUserService
{
    private readonly HttpClient _client;

    public NovelUserService(HttpClient client)
    {
        _client = client;
    }

    public async Task<NovelUserDto[]> GetNovelUsersByNovelId(int novelId)
    {
        try
        {
            var novels = await _client.GetFromJsonAsync<NovelUserDto[]>($"api/NovelUser/{novelId}")!;
            
            if (novels != null)
                return novels;
            
            return Array.Empty<NovelUserDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error : {ex.Message}");
            return Array.Empty<NovelUserDto>();
        }
    }
    
    public async Task<ERemoveNovelUser> RemoveNovelUser(int novelUserId)
    {
        try
        {
            var novels = await _client.DeleteAsync($"api/NovelUser/{novelUserId}")!;
            var responseContent = await novels.Content.ReadAsStringAsync();

            if (responseContent.Contains("Removed"))
            {
                return ERemoveNovelUser.Success;
            }
            
            if (responseContent.Contains("User Already Dont Have Permission On This Novel"))
            {
                return ERemoveNovelUser.AlreadyDontOwnPermission;
            }
            
            if (responseContent.Contains("You Cant Remove Yourself"))
            {
                return ERemoveNovelUser.OwnerTryRemoveItself;
            }
            
            if (responseContent.Contains("There is No NovelUser With This Id")) // will not happen bycouse first check in controller will return it as "You Cant Remove Novel User If You Not Owner Of This Novel"
            {
                return ERemoveNovelUser.ThisNovelUserIdNotExist;
            }
            
            if (responseContent.Contains("You Cant Remove Novel User If You Not Owner Of This Novel"))
            {
                return ERemoveNovelUser.NoPermission;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error : {ex.Message}");
            return ERemoveNovelUser.ServerError;
        }

        return ERemoveNovelUser.UnknownError;
    }
}