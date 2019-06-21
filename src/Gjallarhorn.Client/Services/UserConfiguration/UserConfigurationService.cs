using System.Collections.Generic;
using System.Threading.Tasks;
using Gjallarhorn.Client.Services.Http;
using Gjallarhorn.Client.Services.LocalStorage;
using Gjallarhorn.Client.Storage;
using Gjallarhorn.Shared;

namespace Gjallarhorn.Client.Services.UserConfiguration
{
    public class UserConfigurationService : IUserConfigurationService
    {
        private const string ApiBaseUrl = "api/comparepackage";
        private readonly IHttpClient m_httpClient;
        private readonly ILocalStorage m_localStorage;
        private Gjallarhorn.Shared.UserConfiguration m_userConfiguration;

        public UserConfigurationService(IHttpClientFactory httpClientFactory, ILocalStorage localStorage)
        {
            m_userConfiguration = new Gjallarhorn.Shared.UserConfiguration();
            m_httpClient = httpClientFactory.CreateClient();
            m_localStorage = localStorage;
        }

        public async Task<List<Package>> GetPackages()
        {
            var packages = new List<Package>();
            m_userConfiguration = await GetUserConfiguration();
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

        public async Task<Gjallarhorn.Shared.UserConfiguration> GetUserConfiguration()
        {
            var userConfiguration = await m_localStorage.GetItem<Gjallarhorn.Shared.UserConfiguration>(StorageConstants.Key);
            if (userConfiguration == null)
            {
                return new Gjallarhorn.Shared.UserConfiguration()
                {
                    SourceComparers = new List<SourceComparer>()
                    {
                        new SourceComparer()
                        {
                            SourceA = "https://api.nuget.org/v3/",
                            SourceAAlias = "NuGet",
                            SourceB = "https://api.nuget.org/v3/",
                            SourceBAlias = "Imaginary NuGet Source",
                            Packages = new List<Package>()
                            {
                                new Package()
                                {
                                    Name = "LightInject" ,
                                    ComparePreRelease = false
                                },
                            }
                        }
                    }
                };
            }
            return userConfiguration;
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

        public async Task SaveUserConfiguration(Gjallarhorn.Shared.UserConfiguration userConfiguration)
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