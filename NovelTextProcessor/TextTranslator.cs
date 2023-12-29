using Newtonsoft.Json.Linq;
using NovelTextProcessor.Dtos;
using NovelTextProcessor.Helpers;
using System.Net.Http.Headers;

namespace NovelTextProcessor
{
	public class TextTranslator // TDO Use Paging to control Amount Of Thereds
	{
		private readonly HttpClient httpClient;

		public TextTranslator()
		{
			this.httpClient = ThreadSafeHttpClientSingleton.Instance.GetHttpClient();
		}
		public static TextTranslator Instance { get; } = new TextTranslator();

		int _retry = 0;
		int _maxRetry = 20;

		public async Task<IEnumerable<string>> SendRequestsAsync(IEnumerable<string> arrayOfText)
		{
			var tasks = new List<Task<string>>(); // Use List<Task<string>> for asynchronous tasks

			foreach (var text in arrayOfText)
			{
				var task = Task.Run(() => SendRequestAsync(text));
				tasks.Add(task);
			}

			// Wait for all tasks to complete
			await Task.WhenAll(tasks);

			// Retrieve results from completed tasks
			var results = tasks.Select(task => task.Result);

			return results;
		}

		public async Task<string> SendRequestAsync(string Text)
		{
			var timeTicks = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
			var verifyToken = $"webkey_E3sTuMjpP8Jez49GcYpDVH7r#{timeTicks}#{Text}";

			var hash = MD5Helper.NewMD5(verifyToken);

			var requestBody = new TranslationRequestDto
			{
				multiline = true,
				source = "en", //if enpty api accept any lang
				target = "ar",
				q = Text,
				hints = "",
				ts = timeTicks,
				verify = hash
			};

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

			request.Content = new StringContent(JObject.FromObject(requestBody).ToString());
			request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			try
			{
				var response = await httpClient.SendAsync(request);
				var responseBody = await response.Content.ReadAsStringAsync();

				if (response.StatusCode == System.Net.HttpStatusCode.OK)
				{
					_retry = 0; // Reset Retry
					JObject jsonObject = JObject.Parse(responseBody);

					JToken translationToken = jsonObject.SelectToken("data.translation")!;
					return translationToken!.ToString();
				}
				else if (_retry >= _maxRetry)
				{
					return null!;
				}
			}
			catch (Exception ex)
			{
				File.AppendAllText("Exception.txt", ex.Message + Environment.NewLine);
			}
			finally
			{
				_retry++;
			}
			return await SendRequestAsync(Text);
		}
	}
}
