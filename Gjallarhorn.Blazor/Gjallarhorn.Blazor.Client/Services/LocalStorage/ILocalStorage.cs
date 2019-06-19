using System.Threading.Tasks;

namespace Gjallarhorn.Blazor.Client.Services.LocalStorage
{
    public interface ILocalStorage
    {
        Task<T> GetItem<T>(string key);
        Task SetItem<TItem>(string key, TItem item);
    }
}