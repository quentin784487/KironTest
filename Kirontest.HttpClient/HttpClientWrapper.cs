using Kirontest.HttpClient.Contracts;
using System.Net.Http.Json;

namespace Kirontest.HttpClient
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly System.Net.Http.HttpClient _httpClient;

        public HttpClientWrapper(System.Net.Http.HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> GetAsync<T>(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();            
        }

        public async Task<T> GetAsync<T>(string url, string key, string value)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add(key, value);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}
