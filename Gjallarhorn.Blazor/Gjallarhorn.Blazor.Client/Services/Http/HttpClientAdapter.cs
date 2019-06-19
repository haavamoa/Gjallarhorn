using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Gjallarhorn.Blazor.Client.Services.Http
{
    public class HttpClientAdapter : IHttpClient
    {
        private readonly HttpClient m_httpClient;

        public HttpClientAdapter(HttpClient httpClient)
        {
            m_httpClient = httpClient;
        }

        public async Task<T> PostJsonAsync<T>(string url, T item)
        {
            return await m_httpClient.PostJsonAsync<T>(url, item);
        }
    }
}