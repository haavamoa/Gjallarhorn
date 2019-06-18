using System.Collections.Generic;
using System.Threading.Tasks;
using Gjallarhorn.Blazor.Shared;

namespace Gjallarhorn.Blazor.Client.Services
{
    public interface IUserConfigurationService
    {
        Task<List<Package>> GetPackages();
        Task<UserConfiguration> GetUserConfiguration();
        Task<Package> ComparePackage(Package package);
        Task SavePackage(Package packageModel);
        Task SaveUserConfiguration(UserConfiguration userConfiguration);
        Task DeletePackage(Package package);

    }
}