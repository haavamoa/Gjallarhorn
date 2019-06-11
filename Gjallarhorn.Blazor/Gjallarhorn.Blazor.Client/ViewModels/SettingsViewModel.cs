using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Blazor.Extensions.Storage;
using Gjallarhorn.Blazor.Client.Helpers;
using Gjallarhorn.Blazor.Client.Resources.Commands;
using Gjallarhorn.Blazor.Client.Storage;
using Gjallarhorn.Blazor.Shared;

namespace Gjallarhorn.Blazor.Client.ViewModels
{
    public class SettingsViewModel : IHandlePackages
    {
        private readonly LocalStorage m_localStorage;
        private readonly IPackageFactory m_packageFactory;

        public SettingsViewModel(LocalStorage localStorage, IPackageFactory packageFactory)
        {
            m_localStorage = localStorage;
            m_packageFactory = packageFactory;
            Packages = new List<PackageViewModel>();
            AddNewPackageCommand = new DelegateCommand(async _ => await SaveNewPackage());
        }

        public List<PackageViewModel> Packages { get; set; }
        public string NewPackageName { get; set; }
        public string NewPackageSourceA { get; set; }
        public string NewPackageSourceB { get; set; }
        public bool NewPackageComparePreRelease { get; set; }

        public async Task Initialize()
        {
            var userConfiguration = await m_localStorage.GetItem<UserConfiguration>(StorageConstants.Key);
            Packages = m_packageFactory.CreateViewModels(userConfiguration.Packages);
            Packages.ForEach(p => p.Initialize(this));
        }

        public ICommand AddNewPackageCommand { get; }

        public async Task SaveNewPackage()
        {
            var viewmodel = new PackageViewModel(
                new Package()
                {
                    Name = NewPackageName,
                    SourceA = NewPackageSourceA,
                    SourceB = NewPackageSourceB,
                    ComparePreRelease = NewPackageComparePreRelease
                });
            Packages.Add(viewmodel);
            await m_localStorage.SetItem(StorageConstants.Key, new UserConfiguration(){Packages = m_packageFactory.Create(Packages)});
            viewmodel.Initialize(this);
        }

        public async Task RemovePackage(PackageViewModel packageViewModel)
        {
            Packages.Remove(packageViewModel);
            await m_localStorage.SetItem(StorageConstants.Key, new UserConfiguration() { Packages = m_packageFactory.Create(Packages) });
        }
    }
}