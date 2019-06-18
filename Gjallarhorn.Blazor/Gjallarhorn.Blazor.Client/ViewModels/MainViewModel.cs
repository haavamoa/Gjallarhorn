using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Gjallarhorn.Blazor.Client.Resources.Commands;
using Gjallarhorn.Blazor.Client.Services;
using Gjallarhorn.Blazor.Shared;

namespace Gjallarhorn.Blazor.Client.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IUserConfigurationService m_userConfigurationService;

        public MainViewModel(IUserConfigurationService userConfigurationService)
        {
            m_userConfigurationService = userConfigurationService;
            Packages = new List<PackageViewModel>();
            m_allPackages = Packages;
            ToggleShowLatestCommand = new DelegateCommand(_ => ToggleShowLatest());
        }

        public List<PackageViewModel> Packages { get; set; }
        private bool m_showLatest;
        private List<PackageViewModel> m_allPackages;

        public ICommand ToggleShowLatestCommand { get; }
        public bool ShowLatest  
        {
            get => m_showLatest;
            set => SetProperty(ref m_showLatest, value);
        }

        private void ToggleShowLatest()
        {
            ShowLatest = !ShowLatest;
            Packages = ShowLatest ? m_allPackages.Where(p => p.IsLatest).ToList() : m_allPackages;
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
            m_allPackages = Packages;
        }

        private async Task ComparePackage(PackageViewModel packageViewModel)
        {
            packageViewModel.IsFetching = true;
            OnPropertyChanged(nameof(Packages));
            var updatedPackage = await m_userConfigurationService.ComparePackage(
                new Package()
                {
                    Name = packageViewModel.Name,
                    SourceA = packageViewModel.SourceA,
                    SourceB = packageViewModel.SourceB,
                    ComparePreRelease = packageViewModel.ComparePreRelease
                });

            packageViewModel.UpdatePackage(updatedPackage);
            packageViewModel.IsFetching = false;
            packageViewModel.FetchDate = DateTime.Now;
        }
    }
}