namespace Gjallarhorn.Client.Services.Http
{
    public interface IHttpClientFactory
    {
        IHttpClient CreateClient();
    }
}