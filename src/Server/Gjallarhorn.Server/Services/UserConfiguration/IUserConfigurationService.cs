using System.Collections.Generic;
using System.Threading.Tasks;
using Gjallarhorn.Shared;

namespace Gjallarhorn.Server.Services.UserConfiguration
{
    public interface IUserConfigurationService
    {
        Task<List<Package>> GetPackages();
        Task<Gjallarhorn.Shared.UserConfiguration> GetUserConfiguration();
        Task<Package> ComparePackage(Package package);
        Task SavePackage(Package packageModel);
        Task SaveUserConfiguration(Gjallarhorn.Shared.UserConfiguration userConfiguration);
        Task DeletePackage(Package package);

    }
}