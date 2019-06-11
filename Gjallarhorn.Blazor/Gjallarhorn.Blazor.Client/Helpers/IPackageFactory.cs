using System.Collections.Generic;
using Gjallarhorn.Blazor.Client.ViewModels;
using Gjallarhorn.Blazor.Shared;

namespace Gjallarhorn.Blazor.Client.Helpers
{
    public interface IPackageFactory
    {
        List<PackageViewModel> CreateViewModels(List<Package> packages);
        List<Package> Create(List<PackageViewModel> packageViewModels);
    }
}