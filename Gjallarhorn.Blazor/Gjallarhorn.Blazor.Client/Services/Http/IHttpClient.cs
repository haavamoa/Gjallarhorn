using System.Threading.Tasks;

namespace Gjallarhorn.Blazor.Client.Services.Http
{
    public interface IHttpClient
    {
        Task<T> PostJsonAsync<T>(string url, T item);
    }
}