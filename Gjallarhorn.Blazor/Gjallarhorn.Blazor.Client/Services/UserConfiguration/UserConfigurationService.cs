using System.Collections.Generic;
using System.Threading.Tasks;
using Gjallarhorn.Blazor.Client.Services.Http;
using Gjallarhorn.Blazor.Client.Services.LocalStorage;
using Gjallarhorn.Blazor.Client.Storage;
using Gjallarhorn.Blazor.Shared;

namespace Gjallarhorn.Blazor.Client.Services.UserConfiguration
{
    public class UserConfigurationService : IUserConfigurationService
    {
        private const string ApiBaseUrl = "api/comparepackage";
        private readonly ILocalStorage m_localStorage;
        private readonly IHttpClient m_httpClient;
        private Blazor.Shared.UserConfiguration m_userConfiguration;

        public UserConfigurationService(IHttpClientFactory httpClientFactory, ILocalStorage localStorage)
        {
            m_userConfiguration = new Blazor.Shared.UserConfiguration();
            m_httpClient = httpClientFactory.CreateClient();
            m_localStorage = localStorage;
        }

        public async Task<List<Package>> GetPackages()
        {   
            var packages = new List<Package>();
            m_userConfiguration = await m_localStorage.GetItem<Blazor.Shared.UserConfiguration>(StorageConstants.Key);
            m_userConfiguration.SourceComparers.ForEach(
                s => s.Packages.ForEach(
                    p =>
                    {
                        p.SourceA = s.SourceA;
                        p.SourceB = s.SourceB;
                        p.SourceAAlias = s.SourceAAlias;
                        p.SourceBAlias = s.SourceBAlias;
                    }));
            m_userConfiguration.SourceComparers.ForEach(s => packages.AddRange(s.Packages));
            return packages;
        }

        public async Task<Blazor.Shared.UserConfiguration> GetUserConfiguration()
        {
            return await m_localStorage.GetItem<Blazor.Shared.UserConfiguration>(StorageConstants.Key);
        }

        public async Task<Package> ComparePackage(Package package)
        {
            return await m_httpClient.PostJsonAsync<Package>(ApiBaseUrl + "/comparepackage/", package);
        }

        public async Task SavePackage(Package packageModel)
        {
            AddPackageToUserConfiguration(packageModel);

            await m_localStorage.SetItem(StorageConstants.Key, m_userConfiguration);
        }

        public async Task DeletePackage(Package package)
        {
            m_userConfiguration.SourceComparers.ForEach(s => SearchAndRemovePackage(s, package));
            await m_localStorage.SetItem(StorageConstants.Key, m_userConfiguration);
        }

        public async Task SaveUserConfiguration(Blazor.Shared.UserConfiguration userConfiguration)
        {
            await m_localStorage.SetItem(StorageConstants.Key, userConfiguration);
        }

        private static void SearchAndRemovePackage(SourceComparer sourceComparer, Package package)
        {
            if (sourceComparer.SourceB.Equals(package.SourceA) && sourceComparer.SourceB.Equals(package.SourceB))
            {
                sourceComparer.Packages.Remove(sourceComparer.Packages.Find(p => p.Name.Equals(package.Name)));
            }
        }

        private void AddPackageToUserConfiguration(Package packageModel)
        {
            var wasAdded = false;
            foreach (var sourceComparer in m_userConfiguration.SourceComparers)
            {
                if (!sourceComparer.SourceA.Equals(packageModel.SourceA) && !sourceComparer.SourceB.Equals(packageModel.SourceB))
                    continue;
                sourceComparer.Packages.Add(packageModel);
                wasAdded = true;
            }

            if (!wasAdded)
            {
                m_userConfiguration.SourceComparers.Add(
                    new SourceComparer()
                    {
                        SourceA = packageModel.SourceA, SourceB = packageModel.SourceB, Packages = new List<Package> { packageModel }
                    });
            }
        }
    }
}