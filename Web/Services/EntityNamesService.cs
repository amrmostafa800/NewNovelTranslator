using Newtonsoft.Json;
using System.Net.Http.Json;
using Web.Models;

namespace Web.Services
{
    public class EntityNamesService
    {
        private readonly HttpClient _client;

        public EntityNamesService(HttpClient client)
        {
            _client = client;
        }

        public async Task<EntityName[]> GetAllEntityNamesByNovelId(int novelId)
        {
            try
            {
                var novels = await _client.GetFromJsonAsync<EntityName[]>($"api/EntityName/{novelId}")!;
                if (novels != null)
                {
                    return novels;
                }
                return Array.Empty<EntityName>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching EntityNames: {ex.Message}");
                return Array.Empty<EntityName>();
            }
        }

        public async Task<string[]> ExtractEntityNamesFromText(string text)
        {
            var jsonText = new
            {
                text,
            };

            try
            {
                var novels = await _client.PostAsJsonAsync($"api/EntityName/ExtractEntityNames", jsonText)!;
                if (novels != null)
                {
                    string responseBody = await novels.Content.ReadAsStringAsync();
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
}
