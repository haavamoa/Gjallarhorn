using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace Gjallarhorn.Server.Services.LocalStorage
{
    public class LocalStorageAdapter : ILocalStorage
    {
        private readonly ILocalStorageService m_localStorageService;

        public LocalStorageAdapter(ILocalStorageService localStorageService)
        {
            m_localStorageService = localStorageService;
        }

        public async Task<T> GetItem<T>(string key)
        {
            return await m_localStorageService.GetItemAsync<T>(key);
        }

        public async Task SetItem<TItem>(string key, TItem item)
        {
            await m_localStorageService.SetItemAsync(key, item);
        }
    }
}