namespace Gjallarhorn.Blazor.Client.Services.Http
{
    public interface IHttpClientFactory
    {
        IHttpClient CreateClient();
    }
}