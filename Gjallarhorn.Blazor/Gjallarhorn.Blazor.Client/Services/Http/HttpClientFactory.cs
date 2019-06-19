using System.Net.Http;

namespace Gjallarhorn.Blazor.Client.Services.Http
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly IHttpClient m_httpClient;

        public HttpClientFactory(IHttpClient httpClient)
        {
            m_httpClient = httpClient;
        }

        /// <summary>
        /// Creates a httpclient, this is in lack of IHttpClientFactory in Blazor
        /// </summary>
        /// <returns></returns>
        public IHttpClient CreateClient()
        {
            return m_httpClient;
        }
    }
}
