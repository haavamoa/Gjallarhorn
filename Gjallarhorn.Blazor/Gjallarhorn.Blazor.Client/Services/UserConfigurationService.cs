using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Blazor.Extensions.Storage;
using Gjallarhorn.Blazor.Client.Storage;
using Gjallarhorn.Blazor.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace Gjallarhorn.Blazor.Client.Services
{
    public class UserConfigurationService : IUserConfigurationService
    {
        private const string ApiBaseUrl = "api/comparepackage";
        private readonly HttpClient m_httpClient;
        private readonly LocalStorage m_localStorage;
        private UserConfiguration m_userConfiguration;

        public UserConfigurationService(HttpClient httpClient, LocalStorage localStorage)
        {
            m_httpClient = httpClient;
            m_localStorage = localStorage;
        }

        public async Task<List<Package>> GetPackages()
        {
            var packages = new List<Package>();
            m_userConfiguration = await m_localStorage.GetItem<UserConfiguration>(StorageConstants.Key);
            m_userConfiguration.SourceComparers.ForEach(
                s => s.Packages.ForEach(
                    p =>
                    {
                        p.SourceA = s.SourceA;
                        p.SourceB = s.SourceB;
                    }));
            m_userConfiguration.SourceComparers.ForEach(s => packages.AddRange(s.Packages));
            return packages;
        }

        public async Task<UserConfiguration> GetUserConfiguration()
        {
            return await m_localStorage.GetItem<UserConfiguration>(StorageConstants.Key);
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

        public async Task SaveUserConfiguration(UserConfiguration userConfiguration)
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