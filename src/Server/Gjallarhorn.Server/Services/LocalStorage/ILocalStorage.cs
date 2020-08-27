using System.Threading.Tasks;

namespace Gjallarhorn.Server.Services.LocalStorage
{
    public interface ILocalStorage
    {
        Task<T> GetItem<T>(string key);
        Task SetItem<TItem>(string key, TItem item);
    }
}