using Newtonsoft.Json.Linq;
using NovelTextProcessor.Dtos;
using NovelTextProcessor.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NovelTextProcessor
{
    internal class TextTranslator
    {
        private readonly HttpClient httpClient;

        public TextTranslator()
        {
            this.httpClient = ThreadSafeHttpClientSingleton.Instance.GetHttpClient();
        }
        public static TextTranslator Instance { get; } = new TextTranslator();

        public async Task<IEnumerable<string>> SendRequests(IEnumerable<string> ArrayOfText)
        {
            var tasks = ArrayOfText.Select(text => SendRequestAsync(text));
            var responses = await Task.WhenAll(tasks);
            return responses;
        }

        protected async Task<string> SendRequestAsync(string Text)
        {
            var timeTicks = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var verifyToken = $"webkey_E3sTuMjpP8Jez49GcYpDVH7r#{timeTicks}#{Text}";

            var hash = MD5Helper.NewMD5(verifyToken);

            var requestBody = new TranslationRequestDto
            {
                multiline = true,
                source = "", //if enpty api accept any lang
                target = "ar",
                q = Text,
                hints = "",
                ts = timeTicks,
                verify = hash
            };

            string jsonString = JsonSerializer.Serialize(requestBody);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://webapi.modernmt.com/translate");

            request.Headers.Add("authority", "webapi.modernmt.com");
            request.Headers.Add("accept", "application/json, text/plain, */*");
            request.Headers.Add("accept-language", "ar,en;q=0.9");
            request.Headers.Add("dnt", "1");
            request.Headers.Add("origin", "https://www.modernmt.com");
            request.Headers.Add("referer", "https://www.modernmt.com/");
            request.Headers.Add("sec-ch-ua", "\"Not.A/Brand\";v=\"8\", \"Chromium\";v=\"114\", \"Google Chrome\";v=\"114\"");
            request.Headers.Add("sec-ch-ua-mobile", "?0");
            request.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
            request.Headers.Add("sec-fetch-dest", "empty");
            request.Headers.Add("sec-fetch-mode", "cors");
            request.Headers.Add("sec-fetch-site", "same-site");
            request.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36");
            request.Headers.Add("x-http-method-override", "GET");

            request.Content = new StringContent(jsonString);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await httpClient.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                JObject jsonObject = JObject.Parse(responseBody);

                JToken translationToken = jsonObject.SelectToken("data.translation")!;
                return translationToken!.ToString();
            }
            return null!;
        }
    }
}
