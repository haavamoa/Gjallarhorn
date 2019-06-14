using System.Collections.Generic;
using System.Threading.Tasks;
using Gjallarhorn.Blazor.Shared;

namespace Gjallarhorn.Blazor.Client.Services
{
    public interface IPackagesService
    {
        Task<List<Package>> GetPackages();
        Task<Package> ComparePackage(Package package);
        Task SavePackage(Package packageModel);
        Task DeletePackage(Package package);
    }
}