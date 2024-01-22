using System.Net;
using System.Net.Http.Json;
using Web.Enums;
using Web.Models;

namespace Web.Services;

public class NovelService
{
    private readonly HttpClient _client;

    public NovelService(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<NovelDto>> GetAllNovels()
    {
        try
        {
            var novels = await _client.GetFromJsonAsync<List<NovelDto>>("api/Novel")!;

            if (novels != null)
                return novels;

            return new List<NovelDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error : {ex.Message}");
            return new List<NovelDto>();
        }
    }

    public async Task<ENovelResult> AddNovel(string novelName)
    {
        var addNovel = new
        {
            novelName
        };

        var response = await _client.PostAsJsonAsync("api/Novel", addNovel)!;

        if (response.IsSuccessStatusCode)
            return ENovelResult.Success;

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            return ENovelResult.AuthRequired;

        return ENovelResult.ServerError;
    }

    public async Task<ENovelResult> RemoveNovel(int novelId)
    {

        var response = await _client.DeleteAsync($"api/Novel/{novelId}")!;
        var responseContent = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
            return ENovelResult.Success;

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            return ENovelResult.AuthRequired;

        if (responseContent.Contains("No Novel With This Id"))
        {
            return ENovelResult.NotExist;
        }

        if (responseContent.Contains("You Cant Delete Novel Not Created By You"))
        {
            return ENovelResult.DontOwnPermission;
        }

        return ENovelResult.ServerError;
    }

    public async Task<string> Translate(string text, int novelId)
    {
        var json = new
        {
            text
        };

        try
        {
            var response = await _client.PostAsJsonAsync($"api/Translate/{novelId}", json)!;
            var responseContent = await response.Content.ReadAsStringAsync();

            if (responseContent == "Please Add At Last One Entity Name Before Translate")
            {
                return "Please Add At Last One Entity Name Before Translate";
            }
            if (response.IsSuccessStatusCode)
            {
                return responseContent;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error : {ex.Message}");
            return String.Empty;
        }
        return String.Empty;
    }
}