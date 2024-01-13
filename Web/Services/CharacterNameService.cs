using System.Net.Http.Json;
using Newtonsoft.Json;
using Web.Models;

namespace Web.Services;

public class CharacterNameService
{
    private readonly HttpClient _client;

    public CharacterNameService(HttpClient client)
    {
        _client = client;
    }

    public async Task<CharacterName[]> GetAllEntityNamesByNovelId(int novelId)
    {
        try
        {
            var novels = await _client.GetFromJsonAsync<CharacterName[]>($"api/EntityName/{novelId}")!;
            if (novels != null) return novels;
            return Array.Empty<CharacterName>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching EntityNames: {ex.Message}");
            return Array.Empty<CharacterName>();
        }
    }

    public async Task<string[]> ExtractEntityNamesFromText(string text)
    {
        var jsonText = new
        {
            text
        };

        try
        {
            var novels = await _client.PostAsJsonAsync("api/EntityName/ExtractEntityNames", jsonText)!;
            if (novels != null)
            {
                var responseBody = await novels.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<string[]>(responseBody)!;
            }

            return Array.Empty<string>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching EntityNames: {ex.Message}");
            return Array.Empty<string>();
        }
    }
}