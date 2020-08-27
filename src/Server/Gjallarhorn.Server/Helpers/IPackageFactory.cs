using System.Collections.Generic;
using Gjallarhorn.Server.ViewModels;
using Gjallarhorn.Shared;

namespace Gjallarhorn.Client.Helpers
{
    public interface IPackageFactory
    {
        List<PackageViewModel> CreateViewModels(List<Package> packages);
        List<Package> Create(List<PackageViewModel> packageViewModels);
    }
}