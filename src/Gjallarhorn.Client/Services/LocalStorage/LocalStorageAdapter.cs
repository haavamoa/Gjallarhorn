using System.Threading.Tasks;

namespace Gjallarhorn.Client.Services.LocalStorage
{
    public class LocalStorageAdapter : ILocalStorage
    {   
        private readonly global::Blazor.Extensions.Storage.LocalStorage m_localStorage;

        public LocalStorageAdapter(global::Blazor.Extensions.Storage.LocalStorage localStorage)
        {
            m_localStorage = localStorage;
        }

        public Task<T> GetItem<T>(string key)
        {
            return m_localStorage.GetItem<T>(key);
        }

        public Task SetItem<TItem>(string key, TItem item)
        {
            return m_localStorage.SetItem(key, item);
        }
    }
}