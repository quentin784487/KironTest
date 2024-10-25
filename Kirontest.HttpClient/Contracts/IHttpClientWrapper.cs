namespace Kirontest.HttpClient.Contracts
{
    public interface IHttpClientWrapper
    {
        Task<T> GetAsync<T>(string url);
        Task<T> GetAsync<T>(string url, string key, string value);
    }
}
