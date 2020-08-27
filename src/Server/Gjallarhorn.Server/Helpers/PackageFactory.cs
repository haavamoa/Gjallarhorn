using System.Collections.Generic;
using Gjallarhorn.Server.ViewModels;
using Gjallarhorn.Shared;

namespace Gjallarhorn.Client.Helpers
{
    public class PackageFactory : IPackageFactory
    {
        public List<PackageViewModel> CreateViewModels(List<Package> packages)
        {
            var packageViewModels = new List<PackageViewModel>();
            packages.ForEach(p => packageViewModels.Add(new PackageViewModel(p)));
            return packageViewModels;
        }

        public List<Package> Create(List<PackageViewModel> packageViewModels)
        {
            var packages = new List<Package>();
            packageViewModels.ForEach(p => packages.Add(new Package(){Name = p.Name, SourceA = p.SourceA, SourceB = p.SourceB, ComparePreRelease = p.ComparePreRelease, SourceAVersion = p.SourceAVersion, SourceBVersion = p.SourceBVersion}));
            return packages;
        }
    }
}