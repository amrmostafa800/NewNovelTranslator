using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Web.Dto;
using Web.Models;

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

        public async Task<bool> AddNovel(string novelName)
        {
            AddNovelDto addNovel = new() 
            {
                novelName = novelName
            };

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5000/api/Novel");

            request.Headers.Add("accept", "*/*");

            request.Content = new StringContent(JObject.FromObject(addNovel).ToString());
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await _client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
