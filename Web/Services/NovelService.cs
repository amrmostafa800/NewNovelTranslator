﻿using System.Net;
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

    public async Task<Novel[]> GetAllNovels()
    {
        try
        {
            var novels = await _client.GetFromJsonAsync<Novel[]>("api/Novel")!;
            
            if (novels != null)
                return novels;
            
            return Array.Empty<Novel>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error : {ex.Message}");
            return Array.Empty<Novel>();
        }
    }

    public async Task<EaddNovelResult> AddNovel(string novelName)
    {
        var addNovel = new
        {
            novelName
        };

        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5000/api/Novel");

        request.Headers.Add("accept", "*/*");

        request.Content = new StringContent(JObject.FromObject(addNovel).ToString());
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        var response = await _client.SendAsync(request);
        
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