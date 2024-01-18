using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Newtonsoft.Json.Linq;
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

    public async Task<NovelDto[]> GetAllNovels()
    {
        try
        {
            var novels = await _client.GetFromJsonAsync<NovelDto[]>("api/Novel")!;
            
            if (novels != null)
                return novels;
            
            return Array.Empty<NovelDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error : {ex.Message}");
            return Array.Empty<NovelDto>();
        }
    }

    public async Task<EaddNovelResult> AddNovel(string novelName)
    {
        var addNovel = new
        {
            novelName
        };

        var response = await _client.PostAsJsonAsync("api/Novel",addNovel)!;
        
        if (response.IsSuccessStatusCode)
            return EaddNovelResult.Success;
        
        if (response.StatusCode == HttpStatusCode.Unauthorized)
            return EaddNovelResult.AuthRequired;
        
        return EaddNovelResult.ServerError;
    }

    public async Task<string> Translate(string text,int novelId)
    {
        var json = new
        {
            text
        };
        
        try
        {
            var response = await _client.PostAsJsonAsync($"api/Translate/{novelId}",json)!;
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
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