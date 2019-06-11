using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Blazor.Extensions.Storage;
using Gjallarhorn.Blazor.Client.Helpers;
using Gjallarhorn.Blazor.Client.Resources.Commands;
using Gjallarhorn.Blazor.Client.Storage;
using Gjallarhorn.Blazor.Shared;
using Microsoft.AspNetCore.Components;

namespace Gjallarhorn.Blazor.Client.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly HttpClient m_httpClient;
        private readonly LocalStorage m_localStorage;
        private readonly IPackageFactory m_packageFactory;
        private const string ApiBaseUrl = "api/comparepackage";
        public MainViewModel(HttpClient httpClient, LocalStorage localStorage, IPackageFactory packageFactory)
        {
            m_httpClient = httpClient;
            m_localStorage = localStorage;
            m_packageFactory = packageFactory;
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
            var userConfiguration = await m_localStorage.GetItem<UserConfiguration>(StorageConstants.Key);
            Packages.AddRange(m_packageFactory.CreateViewModels(userConfiguration.Packages));

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
            var updatedPackage = await m_httpClient.PostJsonAsync<Package>(ApiBaseUrl + "/comparepackage/", new Package(){Name = packageViewModel.Name, SourceA = packageViewModel.SourceA, SourceB = packageViewModel.SourceB, ComparePreRelease = packageViewModel.ComparePreRelease});

            packageViewModel.UpdatePackage(updatedPackage);
            packageViewModel.IsFetching = false;
            packageViewModel.FetchDate = DateTime.Now;
        }
    }
}