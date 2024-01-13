namespace NovelTextProcessor;

public sealed class ThreadSafeHttpClientSingleton : IDisposable //This Class By ChatGpt
{
    private static readonly Lazy<ThreadSafeHttpClientSingleton> lazyInstance =
        new(() => new ThreadSafeHttpClientSingleton());

    private readonly HttpClient httpClient;

    private ThreadSafeHttpClientSingleton()
    {
        // Configure HttpClient with necessary settings (e.g., timeout, headers)
        httpClient = new HttpClient();
    }

    public static ThreadSafeHttpClientSingleton Instance => lazyInstance.Value;

    public void Dispose()
    {
        // Dispose of the HttpClient when the application is shutting down
        httpClient.Dispose();
    }

    public HttpClient GetHttpClient()
    {
        // You can customize the HttpClient instance here if needed
        return httpClient;
    }
}