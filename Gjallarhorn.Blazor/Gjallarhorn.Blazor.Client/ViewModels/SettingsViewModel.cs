using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Gjallarhorn.Blazor.Client.Helpers;
using Gjallarhorn.Blazor.Client.Resources.Commands;
using Gjallarhorn.Blazor.Client.Services;
using Gjallarhorn.Blazor.Shared;

namespace Gjallarhorn.Blazor.Client.ViewModels
{
    public class SettingsViewModel : IHandlePackages
    {
        private readonly IPackagesService m_packagesService;
        private UserConfiguration m_userConfiguration;

        public SettingsViewModel(IPackagesService packagesService, IPackageFactory packageFactory)
        {
            m_packagesService = packagesService;
            Packages = new List<PackageViewModel>();
            AddNewPackageCommand = new DelegateCommand(async _ => await SaveNewPackage());
            m_userConfiguration = new UserConfiguration();
        }

        public List<PackageViewModel> Packages { get; set; }
        public string? NewPackageName { get; set; }
        public string? NewPackageSourceA { get; set; }
        public string? NewPackageSourceB { get; set; }
        public bool NewPackageComparePreRelease { get; set; }

        public ICommand AddNewPackageCommand { get; }

        public async Task RemovePackage(PackageViewModel packageViewModel)
        {
            Packages.Remove(packageViewModel);

            await m_packagesService.DeletePackage(packageViewModel.Package);
        }

        public async Task Initialize()
        {
            Packages.Clear();
            var packages = await m_packagesService.GetPackages();
            packages.ForEach(
                p =>
                {
                    var packageViewModel = new PackageViewModel(p);
                    Packages.Add(packageViewModel);
                    packageViewModel.Initialize(this);
                });
        }

        public async Task SaveNewPackage()
        {
            var packageModel = new Package()
            {
                Name = NewPackageName, SourceA = NewPackageSourceA, SourceB = NewPackageSourceB, ComparePreRelease = NewPackageComparePreRelease
            };
            var packageViewModel = new PackageViewModel(packageModel);
            Packages.Add(packageViewModel);

            await m_packagesService.SavePackage(packageModel);

            packageViewModel.Initialize(this);
        }
    }
}