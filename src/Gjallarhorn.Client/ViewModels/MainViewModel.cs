using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Gjallarhorn.Client.Resources.Commands;
using Gjallarhorn.Client.Services.UserConfiguration;

namespace Gjallarhorn.Client.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IUserConfigurationService m_userConfigurationService;

        public MainViewModel(IUserConfigurationService userConfigurationService)
        {
            m_userConfigurationService = userConfigurationService;
            m_packages = new List<PackageViewModel>();
            Packages = new List<PackageViewModel>();
            m_originalPackages = Packages;
            ToggleShowLatestCommand = new DelegateCommand(_ => ToggleShowLatest());
        }

        public List<PackageViewModel> Packages
        {
            get => m_packages;
            set => SetProperty(ref m_packages, value);
        }

        private bool m_showLatest;
        private List<PackageViewModel> m_originalPackages;
        private List<PackageViewModel> m_packages;

        public ICommand ToggleShowLatestCommand { get; }
        public bool ShowLatest  
        {
            get => m_showLatest;
            set => SetProperty(ref m_showLatest, value);
        }

        private void ToggleShowLatest()
        {
            ShowLatest = !ShowLatest;
            Packages = ShowLatest ? m_originalPackages : m_originalPackages.Where(p => p.IsLatest).ToList();
        }

        public async Task Initialize()
        {
            Packages.Clear();

            var packages = await m_userConfigurationService.GetPackages();
            packages.ForEach(p => Packages.Add(new PackageViewModel(p)));

            var comparingTasks = new List<Task>();
            foreach (var packageViewModel in Packages)
            {
                comparingTasks.Add(ComparePackage(packageViewModel));
            }

            IsBusy = true;
            await Task.WhenAll(comparingTasks);
            IsBusy = false;
            m_originalPackages = Packages;
        }

        private async Task ComparePackage(PackageViewModel packageViewModel)
        {
            packageViewModel.IsFetching = true;
            OnPropertyChanged(nameof(Packages));
            var updatedPackage = await m_userConfigurationService.ComparePackage(
                packageViewModel.Package);

            packageViewModel.UpdateVersions(updatedPackage.SourceAVersion, updatedPackage.SourceBVersion);
            packageViewModel.IsFetching = false;
            packageViewModel.FetchDate = DateTime.Now;
            Packages = m_originalPackages.OrderByDescending(p => !p.IsLatest).ToList();
        }
    }
}