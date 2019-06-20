using System.Threading.Tasks;

namespace Gjallarhorn.Client.Services.Http
{
    public interface IHttpClient
    {
        Task<T> PostJsonAsync<T>(string url, T item);
    }
}