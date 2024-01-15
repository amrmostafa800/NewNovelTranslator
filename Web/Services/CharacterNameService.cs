using System.Net.Http.Json;
using Newtonsoft.Json;
using Web.Enums;
using Web.Models;

namespace Web.Services;

public class CharacterNameService
{
    private readonly HttpClient _client;

    public CharacterNameService(HttpClient client)
    {
        _client = client;
    }

    public async Task<EaddEntityNameResult> AddEntityNamesByNovelId(List<AddCharacterName> characterNames,int novelId)
    {
        var json = new
        {
            entityNames = characterNames,
            novelId
        };
        
        try
        {
            var response = await _client.PostAsJsonAsync($"api/EntityName",json)!;
            var responseResult = await response.Content.ReadAsStringAsync();

            if (responseResult.Contains("true"))
            {
                return EaddEntityNameResult.Success;
            } 
            if (responseResult.Contains("One Of EntityNames Or More Exist"))
            {
                return EaddEntityNameResult.IsExist;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error : {ex.Message}");
            return EaddEntityNameResult.ServerError;
        }

        return EaddEntityNameResult.AuthRequired;
    }

    public async Task<bool> UpdateEntityNameById(CharacterName characterName)
    {
        var json = new
        {
            characterName.englishName,
            characterName.gender,
            characterName.arabicName
        };
        
        try
        {
            var response = await _client.PutAsJsonAsync($"api/EntityName/{characterName.Id}",json)!;
            
            if (await response.Content.ReadAsStringAsync() == "Edited")
            {
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error : {ex.Message}");
            return false;
        }

        return false;
    }
    
    public async Task<CharacterName[]> GetAllEntityNamesByNovelId(int novelId)
    {
        try
        {
            var novels = await _client.GetFromJsonAsync<CharacterName[]>($"api/EntityName/{novelId}")!;
            
            if (novels != null) 
                return novels;
            
            return Array.Empty<CharacterName>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error : {ex.Message}");
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
            Console.WriteLine($"Error : {ex.Message}");
            return Array.Empty<string>();
        }
    }
}