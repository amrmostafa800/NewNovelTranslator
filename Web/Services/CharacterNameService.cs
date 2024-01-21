using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
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

    public async Task<EEntityNameResult> AddEntityNamesByNovelId(List<AddCharacterNameDto> characterNames, int novelId)
    {
        var json = new
        {
            entityNames = characterNames,
            novelId
        };

        try
        {
            var response = await _client.PostAsJsonAsync($"api/EntityName", json)!;
            var responseResult = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return EEntityNameResult.Success;
            }
            if (responseResult.Contains("One Of EntityNames Or More Exist"))
            {
                return EEntityNameResult.IsExist;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error : {ex.Message}");
            return EEntityNameResult.ServerError;
        }

        return EEntityNameResult.AuthRequired;
    }

    public async Task<EEntityNameResult> RemoveEntityNameById(int entityNameId)
    {
        try
        {
            var response = await _client.DeleteAsync($"api/EntityName/{entityNameId}")!;
            var responseResult = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return EEntityNameResult.Success;
            }
            if (responseResult.Contains("You Dont Have Permission On This Novel"))
            {
                return EEntityNameResult.AuthRequired;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error : {ex.Message}");
            return EEntityNameResult.ServerError;
        }

        return EEntityNameResult.AuthRequired;
    }

    public async Task<EEntityNameResult> UpdateEntityNameById(CharacterNameDto characterNameDto)
    {
        var json = new
        {
            characterNameDto.englishName,
            characterNameDto.gender,
            characterNameDto.arabicName
        };

        try
        {
            var response = await _client.PutAsJsonAsync($"api/EntityName/{characterNameDto.Id}", json)!;
            var responseResult = await response.Content.ReadAsStringAsync();

            if (responseResult.Contains("Edited"))
            {
                return EEntityNameResult.Success;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error : {ex.Message}");
            return EEntityNameResult.ServerError;
        }

        return EEntityNameResult.NoPermission;
    }

    public async Task<List<CharacterNameDto>> GetAllEntityNamesByNovelId(int novelId)
    {
        try
        {
            var novels = await _client.GetFromJsonAsync<CharacterNameDto[]>($"api/EntityName/{novelId}")!;

            if (novels != null)
                return novels.ToList();

            return new List<CharacterNameDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error : {ex.Message}");
            return new List<CharacterNameDto>();
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