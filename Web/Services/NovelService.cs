using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using Web.Models;
using static MudBlazor.Icons;

namespace Web.Services
{
    public class NovelService
    {
        private readonly HttpClient _client;

        public NovelService(HttpClient client)
        {
            _client = client;
        }

        public async Task<Novel[]> GetAllNovels() 
        {
            try
            {
                var novels = await _client.GetFromJsonAsync<Novel[]>($"api/Novel")!;
                if (novels != null)
                {
                    return novels;
                }
                return Array.Empty<Novel>();
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                Console.WriteLine($"Error fetching novels: {ex.Message}");
                return Array.Empty<Novel>();
            }
        }
    }
}
