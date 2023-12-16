namespace NovelTextProcessor
{
    public sealed class ThreadSafeHttpClientSingleton : IDisposable //This Class By ChatGpt (not tested Yet)
    {
        private static readonly Lazy<ThreadSafeHttpClientSingleton> lazyInstance =
            new Lazy<ThreadSafeHttpClientSingleton>(() => new ThreadSafeHttpClientSingleton());

        public static ThreadSafeHttpClientSingleton Instance => lazyInstance.Value;

        private readonly HttpClient httpClient;

        private ThreadSafeHttpClientSingleton()
        {
            // Configure HttpClient with necessary settings (e.g., timeout, headers)
            this.httpClient = new HttpClient();
        }

        public HttpClient GetHttpClient()
        {
            // You can customize the HttpClient instance here if needed
            return httpClient;
        }

        public void Dispose()
        {
            // Dispose of the HttpClient when the application is shutting down
            httpClient.Dispose();
        }
    }
}
