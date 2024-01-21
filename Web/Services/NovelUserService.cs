using System.Net.Http.Json;
using Web.Enums;
using Web.Models;
using Web.Responses;

namespace Web.Services;

public class NovelUserService
{
    private readonly HttpClient _client;

    public NovelUserService(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<NovelUserDto>> GetNovelUsersByNovelId(int novelId)
    {
        try
        {
            var novels = await _client.GetFromJsonAsync<List<NovelUserDto>>($"api/NovelUser/{novelId}")!;

            if (novels != null)
                return novels;

            return new List<NovelUserDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error : {ex.Message}");
            return new List<NovelUserDto>();
        }
    }

    public async Task<Tuple<EAddNovelUserResult, int?>> AddNovelUser(int novelId, string username)
    {
        var json = new
        {
            novelId,
            username
        };

        try
        {
            var novels = await _client.PostAsJsonAsync($"api/NovelUser", json)!;
            var responseContent = await novels.Content.ReadFromJsonAsync<AddNovelUserResponse>();

            if (responseContent!.description.Contains("Added"))
            {
                return new Tuple<EAddNovelUserResult, int?>(EAddNovelUserResult.Success, responseContent.id);
            }
            if (responseContent!.description.Contains("User Already Have Permission On This Novel"))
            {
                return new Tuple<EAddNovelUserResult, int?>(EAddNovelUserResult.AlreadyOwnPermission, 0);
            }
            if (responseContent!.description.Contains("This Username Not Exist"))
            {
                return new Tuple<EAddNovelUserResult, int?>(EAddNovelUserResult.UsernameNotExist, 0);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error : {ex.Message}");
            return new Tuple<EAddNovelUserResult, int?>(EAddNovelUserResult.ServerError, 0);
        }

        return new Tuple<EAddNovelUserResult, int?>(EAddNovelUserResult.UnknownError, 0);
    }

    public async Task<ERemoveNovelUser> RemoveNovelUser(int novelId, int novelUserId)
    {
        try
        {
            var novels = await _client.DeleteAsync($"api/NovelUser/{novelId}/{novelUserId}")!;
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